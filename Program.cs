using System.Net;

// Checks if a .NET HttpClient uses proxy by routing the HTTP request to a closed port on the local machine

Boolean useProxy = true;
String proxyUrl = "http://127.0.0.1:9999"; // Read from appsettings key ProxyUrl
String baseAddress = "https://google.com";

Console.WriteLine($"useProxy: {useProxy}");
Console.WriteLine($"proxyUrl: {proxyUrl}");
Console.WriteLine($"baseAddress: {baseAddress}");

WebProxy proxy = new WebProxy()
{
    Address = new Uri($"{proxyUrl}"),
    BypassProxyOnLocal = true,
};

HttpClientHandler clientHandler = new HttpClientHandler()
{
    Proxy = proxy,
    UseProxy = useProxy, // Read from appsettings key UseProxy
};

HttpClient httpClient = new HttpClient(handler: clientHandler)
{
    BaseAddress = new Uri(baseAddress),
};

try
{
    var result = await httpClient.GetAsync(httpClient.BaseAddress);
    Console.WriteLine($"\nUsing proxy: False (StatusCode: {result.StatusCode})");
}
catch (System.Net.Http.HttpRequestException e)
{
    if (e.Message.Contains("No connection could be made because the target machine actively refused it."))
    {
        Console.WriteLine("\nUsing proxy: True");
    }
    else
    {
        throw e;
    }
}