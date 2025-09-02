namespace MqttDemo.Models;

public record Acknowledgement(string CorrelationId, string NodeId, string Status, string? Note);
