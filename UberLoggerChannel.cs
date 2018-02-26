using System.Collections;
using System.Collections.Generic;
using UberLogger;
using UnityEngine;

/// <summary>
/// Wraps access to a named channel in a class so that it can be used without having to
/// type the channel name each time to enforcing channel name checking at compile-time.
/// </summary>
public class UberLoggerChannel
{
    private string _channelName;
    public UberLoggerChannel(string channelName)
    {
        _channelName = channelName;
    }

    /// <summary>
    /// Gets or sets whether messages sent to this channel should actually be relayed to the logging system or not.
    /// </summary>
    public bool Mute { get; set; }

    [StackTraceIgnore]
    public void Log(string message, params object[] par)
    {
        if (!Mute)
        {
            UberDebug.LogChannel(_channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void Log(Object context, string message, params object[] par)
    {
        if (!Mute)
        {
            UberDebug.LogChannel(context, _channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogWarning(string message, params object[] par)
    {
        if (!Mute)
        {
            UberDebug.LogWarningChannel(_channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogWarning(Object context, string message, params object[] par)
    {
        if (!Mute)
        {
            UberDebug.LogWarningChannel(context, _channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogError(string message, params object[] par)
    {
        if (!Mute)
        {
            UberDebug.LogErrorChannel(_channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogError(Object context, string message, params object[] par)
    {
        if (!Mute)
        {
            UberDebug.LogErrorChannel(context, _channelName, message, par);
        }
    }
}
