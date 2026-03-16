var builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
       .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapGet("/",()=>"Gateway app is running on Azure container app");
app.MapGet("/health",() => Results.Ok("OK"));
app.MapReverseProxy();

app.Run();
