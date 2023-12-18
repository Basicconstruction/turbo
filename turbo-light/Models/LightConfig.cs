using Newtonsoft.Json;

namespace turbo_light.Models;

public class LightConfig
{
    [JsonProperty("pool_port")]
    public int? TurboPoolPort
    {
        get;
        set;
    }
    [JsonProperty("proxy_port")]
    public int? TurboProxyPort
    {
        get;
        set;
    }

    [JsonProperty("pool_start")]
    public bool? StartTurboPool
    {
        get;
        set;
    }
    [JsonProperty("proxy_start")]
    public bool? StartTurboProxy
    {
        get;
        set;
    }
}