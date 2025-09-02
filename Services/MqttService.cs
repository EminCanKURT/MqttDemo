using System.Buffers;              // ReadOnlySequence<byte> için
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Protocol;
using MqttDemo.Options;

namespace MqttDemo.Services;

public class MqttService
{
    private readonly IMqttClient _client;
    private readonly MqttOptions _opt;

    public MqttService(IOptions<MqttOptions> options)
    {
        _opt = options.Value;
        _client = new MqttClientFactory().CreateMqttClient();
    }

    public async Task EnsureConnectedAsync()
    {
        if (_client.IsConnected) return;

        var builder = new MqttClientOptionsBuilder()
            .WithClientId(_opt.ClientId)
            .WithTcpServer(_opt.Host, _opt.Port)
            .WithCleanSession(false)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(30));

        if (!string.IsNullOrEmpty(_opt.Username))
            builder = builder.WithCredentials(_opt.Username, _opt.Password);

        if (_opt.UseTls)
        {
            builder = builder.WithTlsOptions(o =>
            {
                o.UseTls();
            });
        }

        await _client.ConnectAsync(builder.Build(), CancellationToken.None);
    }

    public async Task PublishJsonAsync<T>(
        string topic,
        T payload,
        MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce,
        bool retain = false)
    {
        await EnsureConnectedAsync();

        var json = JsonSerializer.Serialize(payload);
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(Encoding.UTF8.GetBytes(json))
            .WithQualityOfServiceLevel(qos)
            .WithRetainFlag(retain)
            .Build();

        await _client.PublishAsync(msg, CancellationToken.None);
    }

    public async Task SubscribeAsync(string topicFilter, Func<string, string, Task> onMessage)
    {
        await EnsureConnectedAsync();

        _client.ApplicationMessageReceivedAsync += async e =>
        {
            var topic = e.ApplicationMessage.Topic;

            ReadOnlySequence<byte> seq = e.ApplicationMessage.Payload;
            var payload = Encoding.UTF8.GetString(seq.ToArray());

            await onMessage(topic, payload);
        };

        await _client.SubscribeAsync(
            topicFilter,
            MqttQualityOfServiceLevel.AtLeastOnce,
            CancellationToken.None
        );
    }
}
