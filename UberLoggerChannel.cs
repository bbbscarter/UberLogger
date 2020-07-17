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
    private string ChannelName;
    public UberLoggerChannel(string channelName)
    {
        ChannelName = channelName;
        Filter = Filters.None;
    }

    /// <summary>
    /// Filters for preventing display of certain message types.
    /// </summary>
    [System.Flags]
    public enum Filters
    {
        None = 0,
        Logs = 1 << 0,
        Warnings = 1 << 1,
        Errors = 1 << 2
    }

    /// <summary>
    /// Gets or sets the current filters being applied to this channel. Messages that match the specified set of flags will be ignored.
    /// </summary>
    public Filters Filter { get; set; }

    [StackTraceIgnore]
    public void Log(string message, params object[] par)
    {
        if ((Filter & Filters.Logs) == 0)
        {
            UberDebug.LogChannel(ChannelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void Log(Object context, string message, params object[] par)
    {
        if ((Filter & Filters.Logs) == 0)
        {
            UberDebug.LogChannel(context, ChannelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogWarning(string message, params object[] par)
    {
        if ((Filter & Filters.Warnings) == 0)
        {
            UberDebug.LogWarningChannel(ChannelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogWarning(Object context, string message, params object[] par)
    {
        if ((Filter & Filters.Warnings) == 0)
        {
            UberDebug.LogWarningChannel(context, ChannelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogError(string message, params object[] par)
    {
        if ((Filter & Filters.Errors) == 0)
        {
            UberDebug.LogErrorChannel(ChannelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogError(Object context, string message, params object[] par)
    {
        if ((Filter & Filters.Errors) == 0)
        {
            UberDebug.LogErrorChannel(context, ChannelName, message, par);
        }
    }
}
