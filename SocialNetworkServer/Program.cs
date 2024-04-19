using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SocialNetworkServer.Interfaces;
using SocialNetworkServer.Services;
using SocialNetworkServer.Extentions;
using SocialNetworkServer.SocNetworkDBContext;
using SocialNetworkServer;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>(); // Использование Startup класса
});

var app = builder.Build();

app.Run();
