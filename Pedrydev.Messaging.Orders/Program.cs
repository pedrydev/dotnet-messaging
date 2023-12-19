using MassTransit;
using MongoDB.Driver;
using Pedrydev.Messaging.Orders.Endpoints;
using Pedrydev.Messaging.Orders.Services;

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
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<InventoryService>(x =>
{
    x.BaseAddress = new Uri(builder.Configuration.GetValue<string>("InventoryUrl") ?? throw new Exception());
});
builder.Services.AddSingleton<IMongoDatabase>(_ =>
{
    var client = new MongoClient(builder.Configuration.GetConnectionString("mongo"));
    return client.GetDatabase("ms-orders");
});
builder.Services.AddScoped<OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapOrderEndpoints();

app.Run();
