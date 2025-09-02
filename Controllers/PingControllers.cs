using Microsoft.AspNetCore.Mvc;
using MQTTnet.Protocol;
using MqttDemo.Models;
using MqttDemo.Services;

namespace MqttDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    private readonly MqttService _mqtt;

    public PingController(MqttService mqtt) => _mqtt = mqtt;

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromQuery] string nodeId, [FromBody] PingCommand cmd)
    {
        var env = Envelope<PingCommand>.Create("ping", cmd);
        var topic = $"cmd/demo/{nodeId}/ping";
        await _mqtt.PublishJsonAsync(topic, env, MqttQualityOfServiceLevel.AtLeastOnce);
        return Accepted(new { env.CorrelationId, Topic = topic });
    }
}
