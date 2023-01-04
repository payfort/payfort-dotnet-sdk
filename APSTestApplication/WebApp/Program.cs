using APS.DotNetSDK.Configuration;
using APS.DotNetSDK.Web;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

LoggingConfiguration loggingConfiguration = new LoggingConfiguration(builder.Services, @"SerilogConfig.json", "APSTestApplication");

var certificate = new X509Certificate2("cert filename .pfx format", "pfx password");

var serviceProvider = SdkConfiguration
    .Configure(@"MerchantSdkConfiguration.json",
        loggingConfiguration,
        new ApplePayConfiguration(certificate));

var fingerPrintJavaScript = new HtmlProvider().GetJavaScriptForFingerPrint();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "Home", action = "Index" });

app.Run();
