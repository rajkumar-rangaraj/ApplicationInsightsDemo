namespace ApplicationInsightsDemo;

using Microsoft.ApplicationInsights.DataContracts;
using OpenTelemetry;
using System.Diagnostics;

/// <summary>
/// A OpenTelemetry processor that filters out successful dependency telemetry items.
/// </summary>

internal sealed class SuccessfulDependencyFilter : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        var dependencyTelemetry = activity.ToDependencyTelemetry();

        if (!OKtoSend(dependencyTelemetry)) { return; }

        base.OnEnd(activity);
    }

    private bool OKtoSend(DependencyTelemetry? dependency)
    {
        if (dependency == null) return true;
        return dependency.Success != true;
    }
}