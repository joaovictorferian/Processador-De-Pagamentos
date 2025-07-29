using Application.UseCases.Pagamentos;
using Core.Interfaces;
using Infraestrutura;
using Infraestrutura.Data;
using Infraestrutura.Messaging;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));


builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddSingleton<IMessageQueue>(s =>
{
    var fila = new RabbitMqMessageQueue();
    fila.Initialize(); 
    return fila;
});



builder.Services.AddScoped<PaymentUseCase>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Bem-vindo à API!");

var paymentService = app.Services.CreateScope().ServiceProvider.GetRequiredService<PaymentUseCase>();

_ = Task.Run(() => paymentService.StartPaymentConfirmationListener());

app.Run();
