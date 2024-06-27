var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
var app = builder.Build();

app.UseRouting();
app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}",
  defaults: new { controller = "Home", action = "Index" });


app.MapGet("/", () => "Hello World!");

app.Run();