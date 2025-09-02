using Microsoft.AspNetCore.Mvc;
using MqttDemo.Services;

namespace MqttDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AcksController : ControllerBase
{
    private readonly AckStore _store;

    public AcksController(AckStore store) => _store = store;

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var ack = _store.Get(id);
        return ack is null
            ? NotFound(new { status = "pending" })
            : Ok(ack);
    }
}
