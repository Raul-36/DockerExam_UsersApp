using System.Text;
using DockerExam_UsersApp.Models;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.MapControllers(); 

var factory = new ConnectionFactory()
{
    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST")!,
    UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER")!,
    Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS")!
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "usersQueue",
    durable: true,
    exclusive: false,
    autoDelete: false
);

var consumer = new EventingBasicConsumer(channel);

consumer.Received += async (sender, deliverEventArgs) =>
{
    var message = Encoding.ASCII.GetString(deliverEventArgs.Body.ToArray());
    var user = System.Text.Json.JsonSerializer.Deserialize<User>(message);
    const string connectionString = Environment.GetEnvironmentVariable("MONGO_HOST")!;

    var client = new MongoClient(connectionString);

    var database = client.GetDatabase("MyDatabase");
DockerExam_UsersApp
    var collection = database.GetCollection<User>("InfoDb");

    await collection.InsertOneAsync(user);

    Console.WriteLine($"Pull: '{message}'");
};

channel.BasicConsume(
    queue: "usersQueue",
    autoAck: true,
    consumer: consumer
);

app.UseHttpsRedirection();

app.Run();
