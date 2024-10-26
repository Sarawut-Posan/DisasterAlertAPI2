using API.Middleware;
using Application.Configuration;
using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors());

// Redis cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "DisasterAlert:";
});

// Add Twilio Configuration
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("Twilio"));

// Add LineNotify Configuration
builder.Services.Configure<LineNotifySettings>(
    builder.Configuration.GetSection("LineNotify"));

builder.Services.AddHttpClient<INotificationService, LineNotifyService>();
// Register repositories
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IAlertSettingRepository, AlertSettingRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();

// Register services
builder.Services.AddScoped<IRiskCalculationService, RiskCalculationService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IExternalDataService, ExternalDataService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IMessagingService, TwilioService>();
builder.Services.AddScoped<AlertNotificationService>();

// Register HttpClient for external API calls
builder.Services.AddHttpClient();

var app = builder.Build();

// Add middleware
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