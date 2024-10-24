using DeliveryGrig.Api.Data;
using DeliveryGrig.Api.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<DataContext>();   // регистрирую класс источника данных со временем жизни Singleton
builder.Services.AddSingleton<OrderFilterValidator>();   
builder.Services.AddScoped<OrderFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
