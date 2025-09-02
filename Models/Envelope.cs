namespace MqttDemo.Models;

public record Envelope<T>(string Type, string CorrelationId, DateTime CreatedAtUtc, T Data)
{
    public static Envelope<T> Create(string type, T data, string? correlationId = null)
        => new(type, correlationId ?? Guid.NewGuid().ToString("N"), DateTime.UtcNow, data);
}
