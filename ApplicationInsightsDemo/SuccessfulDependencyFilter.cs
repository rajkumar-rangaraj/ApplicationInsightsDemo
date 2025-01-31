namespace ApplicationInsightsDemo;

using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

/// <summary>
/// A telemetry processor that filters out successful dependency telemetry items.
/// </summary>
public class SuccessfulDependencyFilter : ITelemetryProcessor
{
    private ITelemetryProcessor Next { get; set; }

    public SuccessfulDependencyFilter(ITelemetryProcessor next)
    {
        this.Next = next;
    }

    // Actual logic goes here, this gets invoked for every telemetry item.
    public void Process(ITelemetry item)
    {
        // To filter out an item, return without calling the next processor.
        if (!OKtoSend(item)) { return; }

        this.Next.Process(item);
    }

    private bool OKtoSend(ITelemetry item)
    {
        var dependency = item as DependencyTelemetry;
        if (dependency == null) return true;

        return dependency.Success != true;
    }
}
