using System.Diagnostics;
using Newtonsoft.Json;
using turbo_light.Models;

var configFile = "config.ini";
var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);
LightConfig? lightConfig = null;
var notFound = true;
if (File.Exists(configFilePath))
{
    var jsonContent = File.ReadAllText(configFilePath);
    lightConfig = JsonConvert.DeserializeObject<LightConfig>(jsonContent);
    notFound = false;
}
else
{
    Console.WriteLine("没有找到配置文件，将使用默认配置文件");
}

lightConfig ??= new LightConfig()
{
    StartTurboPool = true,
    StartTurboProxy = true,
    TurboPoolPort = 8887,
    TurboProxyPort = 8888
};
if (notFound)
{
    File.WriteAllText(configFilePath, JsonConvert.SerializeObject(lightConfig,Formatting.Indented));
}

if (lightConfig.StartTurboPool == true)
{
    Process.Start("explorer.exe", $"http://localhost:{lightConfig.TurboPoolPort}");
}
var turboProxyProcess = new Process();
turboProxyProcess.StartInfo.FileName = "turbo-proxy.exe";
turboProxyProcess.StartInfo.Arguments = $"--port={lightConfig.TurboProxyPort}";
turboProxyProcess.StartInfo.UseShellExecute = false;
turboProxyProcess.StartInfo.RedirectStandardOutput = true;
        
var turboPoolProcess = new Process();
turboPoolProcess.StartInfo.FileName = "turbo-pool.exe";
var absolutePath = Path.GetFullPath("wwwroot");
turboPoolProcess.StartInfo.Arguments = $"{lightConfig.TurboPoolPort} {absolutePath}";
turboPoolProcess.StartInfo.UseShellExecute = false;
turboPoolProcess.StartInfo.RedirectStandardOutput = true;
if (lightConfig.StartTurboProxy==true)
{
    Console.WriteLine("Starting Turbo Proxy...");
    turboProxyProcess.Start();
    Console.WriteLine($"turbo-proxy is listening at 0.0.0.0:{lightConfig.TurboProxyPort}, " +
                      $"使用 http://localhost:{lightConfig.TurboProxyPort}来访问");
}
if (lightConfig.StartTurboPool == true)
{
    Console.WriteLine("Starting Turbo Pool...");
    turboPoolProcess.Start();
    Console.WriteLine($"turbo-pool is listening at 0.0.0.0:{lightConfig.TurboPoolPort}, " +
                      $"使用 http://localhost:{lightConfig.TurboPoolPort}来访问");
}
while (NotExit())
{
    if (lightConfig.StartTurboPool==true)
    {
        Console.WriteLine(await turboPoolProcess.StandardOutput.ReadToEndAsync());
    }

    if (lightConfig.StartTurboProxy == true)
    {
        Console.WriteLine(await turboProxyProcess.StandardOutput.ReadToEndAsync());
    }
    
    
}

if (lightConfig.StartTurboPool==true)
{
    turboPoolProcess.WaitForExit();
}

if (lightConfig.StartTurboProxy == true)
{
    turboProxyProcess.WaitForExit();
}


Console.WriteLine("Both processes have exited. Press any key to close this console.");
Console.ReadKey();

bool NotExit()
{
    return (lightConfig!.StartTurboPool == true && turboPoolProcess!.HasExited) ||
           (lightConfig.StartTurboProxy == true && turboProxyProcess!.HasExited);
}