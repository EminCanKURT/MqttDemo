using System.Text.Json;
using MqttDemo.Models;

namespace MqttDemo.Services;

public class AckListener : BackgroundService
{
    private readonly MqttService _mqtt;
    private readonly AckStore _store;

    public AckListener(MqttService mqtt, AckStore store)
    {
        _mqtt = mqtt;
        _store = store;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // tüm ack/demo/+/+ topic’lerini dinle
        await _mqtt.SubscribeAsync("ack/demo/+/+", async (topic, payload) =>
        {
            var ack = JsonSerializer.Deserialize<Acknowledgement>(payload);
            if (ack is not null) _store.Save(ack);
            await Task.CompletedTask;
        });

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
