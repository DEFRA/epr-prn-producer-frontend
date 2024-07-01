using EPR.Producer.PRN.Frontend.UI.Middleware.Auth;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
    options.ForwardedHostHeaderName = builder.Configuration.GetValue<string>("ForwardedHeaders:ForwardedHostHeaderName") ?? default!;
    options.OriginalHostHeaderName = builder.Configuration.GetValue<string>("ForwardedHeaders:OriginalHostHeaderName") ?? default!;
    options.AllowedHosts = (builder.Configuration.GetValue<string>("ForwardedHeaders:AllowedHosts") ?? default!).Split(";");
});
builder.Services.AddHsts(options =>
{
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
var app = builder.Build();

app.UseRouting();
app.UseMiddleware<SecurityHeaderMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { controller = "Home", action = "Index" });


app.MapGet("/", () => "Hello World!");

app.Run();