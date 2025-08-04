using ECommerceApp.Api.Middlewares;
using ECommerceApp.Application.BackgroundServices;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Persistence.Contexts;
using ECommerceApp.Application.Extensions;
using ECommerceApp.Application.Configurations;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AppSettings>(builder.Configuration);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application layer services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpClient();

builder.Services.AddApiMessaging(builder.Configuration);


builder.Services.AddHostedService<ProductSyncBackgroundService>();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Apply pending migrations at startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<PostgreDataContext>();
    dbContext.Database.Migrate();
}

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();