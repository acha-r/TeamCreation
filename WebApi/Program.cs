// /////////////////////////////////////////////////////////////////////////////
// PLEASE DO NOT RENAME OR REMOVE ANY OF THE CODE BELOW. 
// YOU CAN ADD YOUR CODE TO THIS FILE TO EXTEND THE FEATURES TO USE THEM IN YOUR WORK.
// /////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Extensions;
using WebApi.Helpers;

_.__();

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    builder.WebHost.UseUrls("http://localhost:3000");
    builder.WebHost.ConfigureLogging((context, logging) =>
    {
        var config = context.Configuration.GetSection("Logging");
        logging.AddConfiguration(config);
        logging.AddConsole();
        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
        logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
    });

    var services = builder.Services;
    services.AddControllers();
    services.AddSqlite<DataContext>("DataSource=webApi.db");

    services.AddDataProtection().UseCryptographicAlgorithms(
        new AuthenticatedEncryptorConfiguration
        {
            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
        });

    services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = true;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,

            ValidIssuer = "https://localhost:3000/api/player",
            ValidAudience = "https://localhost:3000/api/player",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdf;lkj12qw09po")),
            ClockSkew = TimeSpan.Zero
        };
    });

    services.AddAuthorization();
    services.RegisterServices();

}



var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.EnsureCreated();
}
app.ConfigureException(builder.Environment);

app.UseAuthentication();
app.UseAuthorization();

// configure HTTP request pipeline
{
    app.MapControllers();
}


app.Run();

public partial class Program { }
