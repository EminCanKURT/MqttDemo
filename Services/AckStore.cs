using System.Collections.Concurrent;
using MqttDemo.Models;

namespace MqttDemo.Services;

public class AckStore
{
    private readonly ConcurrentDictionary<string, Acknowledgement> _acks = new();

    public void Save(Acknowledgement ack) => _acks[ack.CorrelationId] = ack;

    public Acknowledgement? Get(string id) =>
        _acks.TryGetValue(id, out var a) ? a : null;
}
