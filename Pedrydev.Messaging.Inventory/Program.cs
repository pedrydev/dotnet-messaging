using MassTransit;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Pedrydev.Messaging.Inventory.Consumers;
using Pedrydev.Messaging.Inventory.Data;
using Pedrydev.Messaging.Inventory.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureServices((_, services) =>
{
    services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cnf) =>
        {
            cnf.Host(builder.Configuration.GetConnectionString("rabbitmq"), "/", h =>
            {
                h.Username(builder.Configuration.GetValue<string>("Rabbitmq:Username"));
                h.Password(builder.Configuration.GetValue<string>("Rabbitmq:Password"));
            });
            cnf.ConfigureEndpoints(context);
        });
        x.AddInMemoryInboxOutbox();
        x.AddConsumer<OrderCreatedConsumer>();
        x.SetSnakeCaseEndpointNameFormatter();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(x =>
{
    var dataSource = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("pg"))
        .Build();
    x.UseNpgsql(dataSource);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCategoryEndpoints();

app.MapProductEndpoints();

await app.RunAsync();
