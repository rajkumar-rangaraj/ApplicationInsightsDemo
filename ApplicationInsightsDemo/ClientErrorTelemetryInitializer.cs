using OpenTelemetry;
using System.Diagnostics;

namespace ApplicationInsightsDemo;

public sealed class ClientErrorTelemetryInitializer : BaseProcessor<Activity>
{
    public override void OnEnd(Activity activity)
    {
        var requestTelemetry = activity.ToRequestTelemetry();

        if (requestTelemetry != null && int.TryParse(requestTelemetry.ResponseCode, out int code))
        {
            if (code >= 400 && code < 500)
            {
                requestTelemetry.Success = true;
                requestTelemetry.Properties["Overridden400s"] = "true";
            }
        }

        base.OnEnd(activity);
    }
}