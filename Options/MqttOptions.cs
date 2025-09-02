namespace MqttDemo.Options;

public class MqttOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 1883;
    public string ClientId { get; set; } = "demo-api";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool UseTls { get; set; } = false;
}
