using Microsoft.EntityFrameworkCore;
using walletappbackend.Context;
using walletappbackend.Services;
using FluentValidation.AspNetCore;
using walletappbackend.Validators;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("walletappDb")));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SeasonService>();
builder.Services.AddScoped<DailyPointsService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<PaymentHistoryService>();
builder.Services.AddScoped<DailyPointsService>();
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<CreateTransactionRequestValidator>();
    });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        var errorCode = HttpStatusCode.InternalServerError; 
        var errorMessage = "An unexpected error occurred.";

        if (exception is ValidationException validationException)
        {
            errorCode = HttpStatusCode.BadRequest;
            errorMessage = validationException.Message;
        }

        var errorResponse = new
        {
            errorCode = (int)errorCode,
            message = errorMessage
        };

        context.Response.StatusCode = (int)errorCode;
        await context.Response.WriteAsJsonAsync(errorResponse);
    });
});
app.Run();
