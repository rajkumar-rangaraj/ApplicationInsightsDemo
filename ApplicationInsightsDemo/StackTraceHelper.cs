namespace ApplicationInsightsDemo;

public static class StackTraceHelper
{
    public static string[] GetUsefulStack()
    {
        var stackTrace = Environment.StackTrace;
        string[] methods = stackTrace.Split(new[] { " at " }, StringSplitOptions.None);
        return methods;
    }
}
