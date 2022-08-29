using System.Reflection;
using Microsoft.OpenApi.Models;
using Serilog;
using SteamSharedGamesAPI.Middleware;
using SteamSharedGamesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((hostContext, services, conf) => { conf.WriteTo.Console(); });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Description
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);


    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SteamSharedGames API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Alexander Ormseth",
            Email = "A_ormseth@hotmail.com",
            Url = new Uri("https://github.com/AlexanderOrmseth")
        }
    });
});

builder.Services.AddScoped<SteamApiService>();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();