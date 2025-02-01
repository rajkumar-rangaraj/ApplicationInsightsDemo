using Microsoft.ApplicationInsights.DataContracts;
using System.Diagnostics;

namespace ApplicationInsightsDemo;

public static class ActivityExtensions
{
    public static DependencyTelemetry? ToDependencyTelemetry(this Activity activity)
    {
        if (activity.GetTagItem("shim.dependency") is DependencyTelemetry telemetry)
        {
            return telemetry;
        }

        if (activity.Kind == ActivityKind.Client)
        {
            // Actual, DependencyTelemetry(activity)
            return new DependencyTelemetry();
        }

        return null;
    }

    public static RequestTelemetry? ToRequestTelemetry(this Activity activity)
    {
        if (activity.GetTagItem("shim.request") is RequestTelemetry telemetry)
        {
            return telemetry;
        }

        if (activity.Kind == ActivityKind.Client)
        {
            // Actual, RequestTelemetry(activity)
            return new RequestTelemetry();
        }

        return null;
    }
}
