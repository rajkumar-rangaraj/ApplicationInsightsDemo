using System.Collections;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.DataContracts;

namespace ApplicationInsightsDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TelemetryClient telemetryClient;

    public HomeController(ILogger<HomeController> logger, TelemetryClient telemetryClient)
    {
        _logger = logger;
        this.telemetryClient = telemetryClient;
    }

    public IActionResult Index()
    {
        telemetryClient.TrackEvent("Index page visited");

        var prefixes = new[] { "COR_", "CORECLR_", "DOTNET_" };

        var envVars = from envVar in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>()
                      from prefix in prefixes
                      let key = (envVar.Key as string)?.ToUpperInvariant()
                      let value = envVar.Value as string
                      where key.StartsWith(prefix)
                      orderby key
                      select new KeyValuePair<string, string>(key, value);

        _logger.LogInformation("Initiating a call to bing.com"); // This gets converted to Application Insights TraceTelemetry.
        telemetryClient.TrackTrace("Initiating a call to bing.com", SeverityLevel.Information); // Same as _logger.LogInformation

        using var client = new HttpClient();
        var response = client.GetAsync("https://www.bing.com/").Result;

        _logger.LogInformation($"Bing responded with status code {response.StatusCode}");

        return View(envVars.ToList());
    }

    [Route("delay/{seconds}")]
    public IActionResult Delay(int seconds)
    {
        telemetryClient.TrackEvent("Delay page visited", new Dictionary<string, string> { { "seconds", seconds.ToString() } });
        ViewBag.StackTrace = StackTraceHelper.GetUsefulStack();
        Thread.Sleep(TimeSpan.FromSeconds(seconds));
        return View(seconds);
    }

    [Route("delay-async/{seconds}")]
    public async Task<IActionResult> DelayAsync(int seconds)
    {
        telemetryClient.TrackEvent("DelayAsync page visited", new Dictionary<string, string> { { "seconds", seconds.ToString() } });

        ViewBag.StackTrace = StackTraceHelper.GetUsefulStack();
        await Task.Delay(TimeSpan.FromSeconds(seconds));
        return View("Delay", seconds);
    }

    [Route("bad-request")]
    public IActionResult ThrowException()
    {
        telemetryClient.TrackEvent("BadRequest page visited");
        throw new Exception("This was a bad request.");
    }

    [Route("status-code/{statusCode}")]
    public string StatusCodeTest(int statusCode)
    {
        telemetryClient.TrackEvent("StatusCodeTest page visited", new Dictionary<string, string> { { "statusCode", statusCode.ToString() } });
        HttpContext.Response.StatusCode = statusCode;
        return $"Status code has been set to {statusCode}";
    }

    [Route("alive-check")]
    public string IsAlive()
    {
        telemetryClient.TrackEvent("IsAlive page visited");
        return "Yes";
    }
}
