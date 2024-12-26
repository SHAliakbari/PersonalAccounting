﻿using System.Globalization;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Domain.Data;
using System.IO.Compression;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Data.Sqlite;
using PersonalAccounting.Domain.Reports;



namespace PersonalAccounting.BlazorApp.Services
{
    public class BotService : IDisposable
    {
        private readonly ITelegramBotClient telegramBotClient;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<BotService> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly HtmlGenerator htmlGenerator;

        public BotService(ITelegramBotClient telegramBotClient,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            ILogger<BotService> logger,
            IWebHostEnvironment webHostEnvironment,
            HtmlGenerator htmlGenerator
            )
        {
            this.telegramBotClient=telegramBotClient;
            this.dbContext=dbContext;
            this.userManager=userManager;
            this.logger=logger;
            this.webHostEnvironment=webHostEnvironment;
            this.htmlGenerator=htmlGenerator;
        }

        public void Dispose()
        {

        }

        public async Task HandleMessage(Update update)
        {
            if (update.Message is null) return;         // we want only updates about new Message
            if (string.IsNullOrEmpty(update.Message.Chat.Username)) return;
            //if (update.Message.Text is null) return;    // we want only updates about new Text Message
            if (update.Message.Text is not null)
            {
                var msg = update.Message;
                Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");
                // let's echo back received text in the chat

                var users = dbContext.Users.Where(x => x.TelegramUser.ToLower() == msg.Chat.Username!.ToLower());
                if (users.Count() > 1)
                {
                    logger.LogInformation($"User {msg.Chat.Username} duplicate");
                    return;
                }
                if (users.Count() == 0)
                {
                    logger.LogInformation($"User {msg.Chat.Username} not found");
                    return;
                }
                ApplicationUser user = users.First();

                if (await userManager.IsInRoleAsync(user, "admin"))
                {
                    if (msg.Text.ToLower().Trim() == "/backup")
                    {
                        //do the backup
                        string fileName = $"{DateTime.UtcNow.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_")}.zip";
                        using MemoryStream fileStream = new MemoryStream(await DownloadZippedFile("dbfile.db", fileName));
                        await telegramBotClient.MakeRequestAsync(
                            new Telegram.Bot.Requests.SendDocumentRequest()
                            {
                                ChatId =  msg.Chat.Id,
                                Document =  new Telegram.Bot.Types.InputFileStream(fileStream, fileName),
                                Caption = $"{DateTime.UtcNow.ToString()}",
                            });
                        return;
                    }
                    if (msg.Text.ToLower().Trim().StartsWith("/report"))
                    {
                        if (msg.Text.Length <=8)
                        {
                            await telegramBotClient.SendTextMessageAsync(msg.Chat, "no username");
                            return;
                        }
                        var parts = msg.Text.ToLower().Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 3)
                        {
                            await telegramBotClient.SendTextMessageAsync(msg.Chat, "no username and switch(0,1) for include desc");
                            return;
                        }
                        var userName = parts[1];
                        if (string.IsNullOrEmpty(userName))
                        {
                            await telegramBotClient.SendTextMessageAsync(msg.Chat, "no username");
                            return;
                        }
                        var items = await dbContext.TransferRequests.Where(x => x.ReceiverUserName.ToLower() == userName || x.ToUserName.ToLower() == userName).ToListAsync();
                        items = calculateReport(items, userName);
                        var html = await GenerateReport(items, parts[2] == "1");
                        //await telegramBotClient.SendTextMessageAsync(msg.Chat, text);
                        string fileName = $"Report - {DateOnly.FromDateTime(DateTime.Now).ConvertToPersianCalendar()} - {userName}.html";
                        using MemoryStream fileStream = new MemoryStream(html);
                        await telegramBotClient.MakeRequestAsync(
                            new Telegram.Bot.Requests.SendDocumentRequest()
                            {
                                ChatId =  msg.Chat.Id,
                                Document =  new Telegram.Bot.Types.InputFileStream(fileStream, fileName),
                                Caption = fileName,
                            });
                    }
                }

                if (msg.Text.ToLower().Trim() == "/report")
                {
                    var items = await dbContext.TransferRequests.Where(x => x.ReceiverUserName.ToLower() == user.Email.ToLower() || x.ToUserName.ToLower() == user.Email.ToLower()).ToListAsync();
                    items = calculateReport(items, user.Email);
                    var html = await GenerateReport(items);
                    string fileName = $"Report - {DateOnly.FromDateTime(DateTime.Now).ConvertToPersianCalendar()} - {user.Email}.html";
                    using MemoryStream fileStream = new MemoryStream(html);
                    await telegramBotClient.MakeRequestAsync(
                        new Telegram.Bot.Requests.SendDocumentRequest()
                        {
                            ChatId =  msg.Chat.Id,
                            Document =  new Telegram.Bot.Types.InputFileStream(fileStream, fileName),
                            Caption = fileName,
                        });

                    //await telegramBotClient.SendTextMessageAsync(msg.Chat, text);
                }
                else if (msg.Text.ToLower().Trim().StartsWith("/echo"))
                {
                    await telegramBotClient.SendTextMessageAsync(msg.Chat, $"{msg.From} said: {msg.Text.Substring(6).Trim()}");
                }
            }
        }

        private List<TransferRequest> calculateReport(List<TransferRequest> transferRequests, string reportSide)
        {
            transferRequests = transferRequests.OrderBy(x => x.RequestDate).ToList();
            decimal sum = 0;
            foreach (var transferRequest in transferRequests)
            {
                transferRequest.Debit = Math.Round(transferRequest.ReceiverUserName == reportSide ? transferRequest.DestinationAmount : 0, 2);
                transferRequest.Credit = Math.Round(transferRequest.ReceiverUserName == reportSide ? 0 : transferRequest.DestinationAmount, 2);
                sum += Math.Round(transferRequest.Debit - transferRequest.Credit, 2);
                transferRequest.RunningTotal = sum;
            }
            return transferRequests;
        }

        private async Task<byte[]> GenerateReport(List<TransferRequest> transferRequests, bool includeDesc = false)
        {

            var res = await htmlGenerator.RenderAndExport(@"Statement.razor", new StatementModel()
            {
                Requests = transferRequests
            });

            return System.Text.ASCIIEncoding.UTF8.GetBytes(res);


            //StringBuilder stringBuilder = new StringBuilder();
            //foreach (var transferRequest in transferRequests)
            //{
            //    stringBuilder.AppendLine($"Date : {transferRequest.RequestDate} = {transferRequest.RequestDate.ConvertToPersianCalendar()}");
            //    stringBuilder.AppendLine($"Amount : {((transferRequest.Debit > 0 ? 1 : -1)*transferRequest.DestinationAmount).ToString("##,##")}");
            //    stringBuilder.AppendLine($"Remaining : {(transferRequest.RunningTotal != 0 ? transferRequest.RunningTotal.ToString("##,##") : 0)} ");
            //    if (includeDesc && !string.IsNullOrEmpty(transferRequest.ReceiverNote))
            //        stringBuilder.AppendLine($"Note : {transferRequest.ReceiverNote} ");
            //    stringBuilder.AppendLine("------------");
            //}

            //return stringBuilder.ToString();
        }

        public async Task<byte[]?> DownloadZippedFile(string filePath, string zipFileName)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }
            string targetFileNameNew = "";
            string targetFileName = Path.GetTempFileName();
            System.IO.File.Delete(targetFileName);
            try
            {
                using (var sourceConnection = dbContext.Database.GetDbConnection())
                {
                    await sourceConnection.OpenAsync();

                    using (var backupConnection = new SqliteConnection($"Data Source={targetFileName}"))
                    {
                        await backupConnection.OpenAsync();
                        (sourceConnection as SqliteConnection).BackupDatabase(backupConnection);
                        await backupConnection.CloseAsync();
                    }
                    await sourceConnection.CloseAsync();
                    await dbContext.Database.CloseConnectionAsync();
                }

                targetFileNameNew = Path.GetTempFileName();
                System.IO.File.Delete(targetFileNameNew);
                System.IO.File.Copy(targetFileName, targetFileNameNew);


                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create))
                    {
                        var fileNameInZip = Path.GetFileName(filePath); // Use original filename in the zip
                        var zipArchiveEntry = zipArchive.CreateEntry(fileNameInZip);

                        using (var fileStream = System.IO.File.OpenRead(targetFileNameNew))
                        {
                            using (var zipEntryStream = zipArchiveEntry.Open())
                            {
                                await fileStream.CopyToAsync(zipEntryStream);
                            }
                        }
                    }

                    //memoryStream.Position = 0; // Reset position for reading

                    return memoryStream.ToArray();

                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the error, return a user-friendly message)
                logger.LogError($"Error zipping file: {ex.Message}");
                return null;
            }
            finally
            {
                try
                {
                    if (System.IO.File.Exists(targetFileNameNew))
                        System.IO.File.Delete(targetFileNameNew);
                    if (System.IO.File.Exists(targetFileName))
                        System.IO.File.Delete(targetFileName);
                }
                catch
                {

                }

            }
        }
    }
}