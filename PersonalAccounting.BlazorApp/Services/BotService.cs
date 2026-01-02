using System.Globalization;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalAccounting.Domain.Data;
using System.IO.Compression;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Data.Sqlite;
using PersonalAccounting.Domain.Reports;
using PersonalAccounting.BlazorApp.Components.Receipt_Component.Services;

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
        private readonly ReceiptService receiptService;

        public BotService(ITelegramBotClient telegramBotClient,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            ILogger<BotService> logger,
            IWebHostEnvironment webHostEnvironment,
            HtmlGenerator htmlGenerator,
            ReceiptService receiptService
            )
        {
            this.telegramBotClient=telegramBotClient;
            this.dbContext=dbContext;
            this.userManager=userManager;
            this.logger=logger;
            this.webHostEnvironment=webHostEnvironment;
            this.htmlGenerator=htmlGenerator;
            this.receiptService=receiptService;
        }

        public void Dispose()
        {

        }

        public async Task HandleMessage(Update update)
        {
            if (update.Message is null) return;         // we want only updates about new Message
            if (string.IsNullOrEmpty(update.Message.Chat.Username)) return;

            var msg = update.Message;
            Console.WriteLine($"Received message '{msg.Text ?? msg.Caption}' in {msg.Chat}");

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

            if (update.Message.Caption is not null)
            {
                if (await userManager.IsInRoleAsync(user, "admin"))
                {
                    if (msg.Caption?.ToLower().Trim() == "/restore" && msg.Document != null)
                    {
                        //do the restore preparation
                        using (var stream = new MemoryStream())
                        {
                            var file = await telegramBotClient.GetFile(msg.Document.FileId);
                            if (file == null || msg.Document.FileName != "dbfile.db")
                            {
                                logger.LogInformation($"File can not download");
                                return;
                            }
                            await telegramBotClient.DownloadFile(file.FilePath!, stream);
                            System.IO.File.WriteAllBytes(System.IO.Path.Combine(webHostEnvironment.ContentRootPath, "dbfile.db_for_next_start"), stream.ToArray());
                            await telegramBotClient.SendMessage(msg.Chat, "prepared for next start");
                        }
                        return;
                    }
                }
            }
            if (update.Message.Text is not null)
            {
                if (await userManager.IsInRoleAsync(user, "admin"))
                {
                    if (msg.Text?.ToLower().Trim() == "/backup")
                    {
                        //do the backup
                        string fileName = $"{DateTime.UtcNow.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_")}.zip";
                        using MemoryStream fileStream = new MemoryStream(await DownloadZippedFile("dbfile.db", fileName));
                        await telegramBotClient.SendRequest(
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
                            await telegramBotClient.SendMessage(msg.Chat, "no username");
                            return;
                        }
                        var parts = msg.Text.ToLower().Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 3)
                        {
                            await telegramBotClient.SendMessage(msg.Chat, "no username and switch(0,1) for include desc");
                            return;
                        }
                        var userName = parts[1];
                        if (string.IsNullOrEmpty(userName))
                        {
                            await telegramBotClient.SendMessage(msg.Chat, "no username");
                            return;
                        }
                        var items = await dbContext.TransferRequests.Where(x => x.ReceiverUserName.ToLower() == userName || x.ToUserName.ToLower() == userName).ToListAsync();
                        items = calculateReport(items, userName);
                        var html = await GenerateReport(items, parts[2] == "1");
                        //await telegramBotClient.SendTextMessageAsync(msg.Chat, text);
                        string fileName = $"Report - {DateOnly.FromDateTime(DateTime.Now).ConvertToPersianCalendar()} - {userName}.html";
                        using MemoryStream fileStream = new MemoryStream(html);
                        await telegramBotClient.SendRequest(
                            new Telegram.Bot.Requests.SendDocumentRequest()
                            {
                                ChatId =  msg.Chat.Id,
                                Document =  new Telegram.Bot.Types.InputFileStream(fileStream, fileName),
                                Caption = fileName,
                            });
                    }

                    if (msg.Text.ToLower().Trim().StartsWith("/receipts_report"))
                    {
                        if (msg.Text.Length <=8)
                        {
                            await telegramBotClient.SendMessage(msg.Chat, "no username");
                            return;
                        }
                        var parts = msg.Text.ToLower().Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length < 2)
                        {
                            await telegramBotClient.SendMessage(msg.Chat, "no username");
                            return;
                        }
                        var userName = parts[1];
                        if (string.IsNullOrEmpty(userName))
                        {
                            await telegramBotClient.SendMessage(msg.Chat, "no username");
                            return;
                        }

                        var month = parts.Length>2 ? int.Parse(parts[2]) : DateTime.Now.Month;
                        var year = parts.Length>3 ? int.Parse(parts[3]) : DateTime.Now.Year;

                        var (startDate, endDate) = GetMonthStartAndEnd(year, month);

                        await GenerateReceiptReport(msg.Chat.Id, userName, startDate, endDate);
                    }
                }

                if (msg.Text.ToLower().Trim() == "/report")
                {
                    var items = await dbContext.TransferRequests.Where(x => x.ReceiverUserName.ToLower() == user.Email.ToLower() || x.ToUserName.ToLower() == user.Email.ToLower()).ToListAsync();
                    items = calculateReport(items, user.Email);
                    var html = await GenerateReport(items);
                    string fileName = $"Report - {DateOnly.FromDateTime(DateTime.Now).ConvertToPersianCalendar()} - {user.Email}.html";
                    using MemoryStream fileStream = new MemoryStream(html);
                    await telegramBotClient.SendRequest(
                        new Telegram.Bot.Requests.SendDocumentRequest()
                        {
                            ChatId =  msg.Chat.Id,
                            Document =  new Telegram.Bot.Types.InputFileStream(fileStream, fileName),
                            Caption = fileName,
                        });

                    //await telegramBotClient.SendTextMessageAsync(msg.Chat, text);
                }
                if (msg.Text.ToLower().Trim() == "/receipts_report")
                {
                    await GenerateReceiptReport(msg.Chat.Id, user.UserName, DateTime.MinValue, DateTime.MaxValue);
                }
                else if (msg.Text.ToLower().Trim().StartsWith("/echo"))
                {
                    await telegramBotClient.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text.Substring(6).Trim()}");
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

            var res = await htmlGenerator.RenderAndExport(@"Statement.cshtml", new StatementModel()
            {
                Requests = transferRequests
            });

            return System.Text.ASCIIEncoding.UTF8.GetBytes(res);
        }

        private async Task GenerateReceiptReport(ChatId chatId, string userName, DateTime startDate, DateTime endDate)
        {
            var items = await receiptService.GenerateReportAsync(userName, startDate, endDate);

            var res = await htmlGenerator.RenderAndExport(@"ReceiptStatement.cshtml", new ReceiptsStatementModel()
            {
                ReceiptReportItems = items,
                UserName = userName,
                StartDate = startDate,
                EndDate = endDate
            });

            var rpt = System.Text.ASCIIEncoding.UTF8.GetBytes(res);

            string fileName = $"Receipts Report - {DateOnly.FromDateTime(DateTime.Now)} - {userName}.html";
            using MemoryStream fileStream = new MemoryStream(rpt);
            await telegramBotClient.SendRequest(
                new Telegram.Bot.Requests.SendDocumentRequest()
                {
                    ChatId = chatId,
                    Document =  new Telegram.Bot.Types.InputFileStream(fileStream, fileName),
                    Caption = fileName,
                });
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

        private (DateTime startDate, DateTime endDate) GetMonthStartAndEnd(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            return (startDate, endDate);
        }
    }
}
