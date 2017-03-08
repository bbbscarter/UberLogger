using UberLogger;

//Helper functions to make logging easier
public static class UberDebug
{
    [StackTraceIgnore]
    static public void Log(UnityEngine.Object context, string message, params object[] par)
    {
        UberLogger.Logger.Log("", context, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void Log(string message, params object[] par)
    {
        UberLogger.Logger.Log("", null, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void LogChannel(UnityEngine.Object context, string channel, string message, params object[] par)
    {
        UberLogger.Logger.Log(channel, context, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void LogChannel(string channel, string message, params object[] par)
    {
        UberLogger.Logger.Log(channel, null, LogSeverity.Message, message, par);
    }


    [StackTraceIgnore]
    static public void LogWarning(UnityEngine.Object context, object message, params object[] par)
    {
        UberLogger.Logger.Log("", context, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarning(object message, params object[] par)
    {
        UberLogger.Logger.Log("", null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarningChannel(UnityEngine.Object context, string channel, string message, params object[] par)
    {
        UberLogger.Logger.Log(channel, context, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarningChannel(string channel, string message, params object[] par)
    {
        UberLogger.Logger.Log(channel, null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogError(UnityEngine.Object context, object message, params object[] par)
    {
        UberLogger.Logger.Log("", context, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogError(object message, params object[] par)
    {
        UberLogger.Logger.Log("", null, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogErrorChannel(UnityEngine.Object context, string channel, string message, params object[] par)
    {
        UberLogger.Logger.Log(channel, context, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogErrorChannel(string channel, string message, params object[] par)
    {
        UberLogger.Logger.Log(channel, null, LogSeverity.Error, message, par);
    }


    //Logs that will not be caught by UberLogger
    //Useful for debugging UberLogger
    [LogUnityOnly]
    static public void UnityLog(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [LogUnityOnly]
    static public void UnityLogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    [LogUnityOnly]
    static public void UnityLogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
}
