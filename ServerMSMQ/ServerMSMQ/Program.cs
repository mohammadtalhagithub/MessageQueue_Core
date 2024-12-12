using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using MsmqDll;
using ServerMSMQ;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var selfhosted = builder.Configuration["SelfHosted"];

if (string.Equals(selfhosted, "true", StringComparison.OrdinalIgnoreCase))
{
    builder.WebHost.ConfigureKestrel(opts =>
    {
        try
        {
            var httpport = builder.Configuration.GetSection("HostingServer:Http:Port");
            opts.ListenAnyIP(int.Parse(httpport.Value));
            var httpsport = builder.Configuration.GetSection("HostingServer:Https:Port");
            opts.ListenAnyIP(int.Parse(httpsport.Value), listenOptions =>
            {
                listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1; // HTTPS port
            });
            //opts.ListenAnyIP(int.Parse(httpsport.Value), opts => opts.UseHttps());


        }
        catch (Exception ex)
        {
            //LogWriterCore.WriteLog("ConfigureKestrel", ex);
            Console.WriteLine(ex.Message);
        }
    });
}
else
{
    builder.WebHost.UseIISIntegration();
}



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<ClientManagement>();

builder.Services.AddControllers();

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
}); // Adjust this to your required size.


builder.Services.AddDistributedMemoryCache();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseResponseCompression();
app.MapControllers();

//app.UseSession();
app.Run();
