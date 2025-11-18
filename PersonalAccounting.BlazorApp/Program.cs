using PersonalAccounting.BlazorApp.Components;
using PersonalAccounting.BlazorApp.Components.Account;
using PersonalAccounting.BlazorApp.Components.NS_TransferRequest.Services;
using PersonalAccounting.Domain.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalAccounting.BlazorApp.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using PersonalAccounting.Domain.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using PersonalAccounting.BlazorApp.Components.Receipt_Component.Services;
using PersonalAccounting.Domain.Services.OCR;
using PersonalAccounting;
using PersonalAccounting.BlazorApp;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Logging.AddAzureWebAppDiagnostics();

//var ocrService = new OCRService(builder.Environment);

//List<string> inputImages = new List<string>
//{
//    @"D:\freshco.jpg",
//    @"D:\BulkBarn.jpg",
//    @"D:\shoppers.jpg"
//};

//foreach (var input in inputImages)
//{
//    var templateName = System.IO.Path.GetFileNameWithoutExtension(input);
//    var path = System.IO.Path.GetPathRoot(input);
//    using (var stream = new MemoryStream(System.IO.File.ReadAllBytes(input)))
//    {
//        await FormRecognizerService.RunAsync(stream, templateName);

//        //MemoryStream finalizedImage = new MemoryStream();
//        //ImagePreprocessor.PreprocessImage(stream, ref finalizedImage);
//        //System.IO.File.WriteAllBytes($"{path}{templateName}_new.jpg", finalizedImage.ToArray());
//        //var content = ocrService.ExtractTextFromImage(finalizedImage.ToArray());
//        //System.IO.File.WriteAllText($"{path}{templateName}.txt", content);

//        //var rec = ocrService.FillTheReceipt(content);

//    }
//}


var token = configuration[$"Authentication_Telegram_BotToken{GlobalConfigs.ProdSuffix}"];             // set your bot token in secrets.json or appsettings
var webhookUrl = configuration[$"Authentication_Telegram_BotWebhookUrl{GlobalConfigs.ProdSuffix}"];   // set your bot webhook public url in secrets.json or appsettings

//#if DEBUG

//builder.Services.ConfigureTelegramBotMvc();

//#endif

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(x => x.DetailedErrors = true);
builder.Services.AddRazorPages().AddViewLocalization().AddDataAnnotationsLocalization();

builder.Services.AddRazorTemplating();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<CustomIdentityService>();
builder.Services.AddScoped<BotService>();
builder.Services.AddScoped<BlobService>();
builder.Services.AddScoped<HtmlGenerator>();
//builder.Services.AddSingleton<RazorViewRenderer>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

//builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfig"));
builder.Services.AddTransient<IEmailSender<ApplicationUser>, EmailSender>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddGoogle(options =>
    {
        options.ClientId = configuration[$"Authentication_Google_ClientId{GlobalConfigs.ProdSuffix}"]!;
        options.ClientSecret = configuration[$"Authentication_Google_ClientSecret{GlobalConfigs.ProdSuffix}"]!;
    })
    //.AddFacebook(options =>
    //{
    //    IConfigurationSection FBAuthNSection =
    //    config.GetSection("Authentication:FB");
    //    options.ClientId = FBAuthNSection["ClientId"];
    //    options.ClientSecret = FBAuthNSection["ClientSecret"];
    //})
    //.AddMicrosoftAccount(microsoftOptions =>
    //{
    //    microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
    //    microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
    //})
    //.AddTwitter(twitterOptions =>
    //{
    //    twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
    //    twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
    //    twitterOptions.RetrieveUserDetails = true;
    //});
    .AddIdentityCookies();

builder.Services.AddAuthorization(options =>
{
    AuthorizationPolicies.RegisterPolicies(builder, options);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<TransferRequestService>();
builder.Services.AddScoped<ReceiptService>();

builder.Services.AddScoped<FormRecognizerService>();

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(token, cancellationToken: cts.Token);
builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>(x => bot);


builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

#if DEBUG

await bot.SetWebhook("");
bot.StartReceiving(updateHandler: new Action<ITelegramBotClient, Update, CancellationToken>(OnUpdateFromPolling), errorHandler: async (bot, ex, ct) =>
{
    await Task.Run(() => Console.WriteLine(ex));
});

#else

await bot.SetWebhook(webhookUrl);

app.MapPost("/bot", async (Update update,IServiceProvider serviceProvider) => {  await HandleUpdateRest(update, serviceProvider); });

#endif

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.UseAuthorization();

RestoreDB(app);

app.MigrateDbContext<ApplicationDbContext>(async (context, services) =>
{
    var rm = services.GetRequiredService<RoleManager<IdentityRole>>();
    if (!await rm.RoleExistsAsync("admin"))
    {
        await rm.CreateAsync(new IdentityRole
        {
            Id="admin",
            Name = "admin",
            NormalizedName = "admin"
        });
    }

    var um = services.GetRequiredService<UserManager<ApplicationUser>>();
    var masterEmail = configuration[$"Authentication_MasterEmail{GlobalConfigs.ProdSuffix}"]!;
    var admin = await um.FindByEmailAsync(masterEmail);
    if (admin != null)
    {
        await um.AddToRoleAsync(admin, "admin");
    }

    var nullRequestDateRequests = context.TransferRequests.Where(x => x.RequestDate == null || x.RequestDate == DateOnly.MinValue);
    foreach (var item in nullRequestDateRequests)
    {
        item.RequestDate = DateOnly.FromDateTime(item.CreateDate);
    }
    await context.SaveChangesAsync();

    var receiptService = services.GetRequiredService<ReceiptService>();
    receiptService.UpdateAdditionDeduction().Wait();
});

app.Run();

async Task HandleUpdateRest(Update update, IServiceProvider serviceProvider)
{
    //if (update.Message is null) return;			// we want only updates about new Message
    //if (update.Message.Text is null) return;	// we want only updates about new Text Message
    //var msg = update.Message;
    //Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");
    //// let's echo back received text in the chat
    //await bot.SendTextMessageAsync(msg.Chat, $"{msg.From} said: {msg.Text}");


    var botService = serviceProvider.GetService<BotService>()!;
    await botService.HandleMessage(update);
}


async void OnUpdateFromPolling(ITelegramBotClient bot, Update update, CancellationToken ct)
{
    //if (update.Message is null) return;			// we want only updates about new Message
    //if (update.Message.Text is null) return;	// we want only updates about new Text Message
    //var msg = update.Message;
    //Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");
    //// let's echo back received text in the chat
    //await bot.SendTextMessageAsync(msg.Chat, $"{msg.From} said: {msg.Text}");

    using var scope = app.Services.CreateScope();
    {
        var botService = scope.ServiceProvider.GetService<BotService>()!;
        await botService.HandleMessage(update);
    }
}

void RestoreDB(WebApplication app)
{
    var fileToRestore = System.IO.Path.Combine(app.Environment.ContentRootPath, "dbfile.db_for_next_start");
    if (System.IO.File.Exists(fileToRestore))
    {
        //restore process 
        var dbFileName = System.IO.Path.Combine(app.Environment.ContentRootPath, "dbfile.db");
        var destBackupFileName = System.IO.Path.Combine(app.Environment.ContentRootPath, $"dbfile_Backup_{DateTime.UtcNow.ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_")}.db");
        if (System.IO.File.Exists(dbFileName))
        {
            System.IO.File.Copy(dbFileName, destBackupFileName);
        }
        System.IO.File.Move(fileToRestore, dbFileName, true);
        System.IO.File.Delete(fileToRestore);
    }
}