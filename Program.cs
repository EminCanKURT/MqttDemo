using MqttDemo.Options;
using MqttDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MqttOptions>(builder.Configuration.GetSection("Mqtt"));
builder.Services.AddSingleton<MqttService>();
builder.Services.AddSingleton<AckStore>();
builder.Services.AddHostedService<AckListener>();   // yalnızca ack listener çalışacak

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
