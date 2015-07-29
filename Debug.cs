using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UberLogger;

public static class Debug
{
    //Unity replacement methods
    [StackTraceIgnore]
    static public void LogFormat(UnityEngine.Object context, string message, params object[] par)
    {
        Logger.Log("", context, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void LogFormat(string message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void Log(object message, UnityEngine.Object context = null)
    {
        Logger.Log("", context, LogSeverity.Message, message);
    }

    [StackTraceIgnore]
    static public void LogErrorFormat(UnityEngine.Object context, string message, params object[] par)
    {
        Logger.Log("", context, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogErrorFormat(string message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogError(object message, UnityEngine.Object context = null)
    {
        Logger.Log("", context, LogSeverity.Error, message);
    }

    [StackTraceIgnore]
    static public void LogWarningFormat(UnityEngine.Object context, string message, params object[] par)
    {
        Logger.Log("", context, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarningFormat(string message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarning(object message, UnityEngine.Object context = null)
    {
        Logger.Log("", context, LogSeverity.Warning, message);
    }
    
    // New methods
    [StackTraceIgnore]
    static public void ULog(UnityEngine.Object context, string message, params object[] par)
    {
        Logger.Log("", context, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void ULog(string message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void ULogChannel(UnityEngine.Object context, string channel, string message, params object[] par)
    {
        Logger.Log(channel, context, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void ULogChannel(string channel, string message, params object[] par)
    {
        Logger.Log(channel, null, LogSeverity.Message, message, par);
    }


    [StackTraceIgnore]
    static public void ULogWarning(UnityEngine.Object context, object message, params object[] par)
    {
        Logger.Log("", context, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void ULogWarning(object message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void ULogWarningChannel(UnityEngine.Object context, string channel, string message, params object[] par)
    {
        Logger.Log(channel, context, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void ULogWarningChannel(string channel, string message, params object[] par)
    {
        Logger.Log(channel, null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void ULogError(UnityEngine.Object context, object message, params object[] par)
    {
        Logger.Log("", context, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void ULogError(object message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void ULogErrorChannel(UnityEngine.Object context, string channel, string message, params object[] par)
    {
        Logger.Log(channel, context, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void ULogErrorChannel(string channel, string message, params object[] par)
    {
        Logger.Log(channel, null, LogSeverity.Error, message, par);
    }


    //Logs that will not be forwarded to UberLogger
    [LogUnityOnly]
    static public void UnityLog(object message)
    {
        UnityEngine.Debug.Log(message);
    }
    [LogUnityOnly]
    static public void UnityLogFormat(string message, params object[] par)
    {
        UnityEngine.Debug.LogFormat(message, par);
    }
}
