using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using UberLogger;

public static class Debug
{
    [StackTraceIgnore]
    static public void LogChannel(string channel, GameObject obj, object message, params object[] par)
    {
        Logger.Log(channel, obj, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void LogChannel(string channel, object message, params object[] par)
    {
        Logger.Log(channel, null, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void Log(GameObject obj, object message, params object[] par)
    {
        Logger.Log("", obj, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void Log(object message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Message, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarningChannel(string channel, GameObject obj, object message, params object[] par)
    {
        Logger.Log(channel, obj, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarningChannel(string channel, object message, params object[] par)
    {
        Logger.Log(channel, null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarning(GameObject obj, object message, params object[] par)
    {
        Logger.Log("", obj, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogWarning(object message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Warning, message, par);
    }

    [StackTraceIgnore]
    static public void LogErrorChannel(string channel, GameObject obj, object message, params object[] par)
    {
        Logger.Log(channel, obj, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogErrorChannel(string channel, object message, params object[] par)
    {
        Logger.Log(channel, null, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogError(GameObject obj, object message, params object[] par)
    {
        Logger.Log("", obj, LogSeverity.Error, message, par);
    }

    [StackTraceIgnore]
    static public void LogError(object message, params object[] par)
    {
        Logger.Log("", null, LogSeverity.Error, message, par);
    }


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
