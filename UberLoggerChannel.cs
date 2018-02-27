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
        Filter = Filters.None;
    }

    /// <summary>
    /// Filters for preventing display of certain message types.
    /// </summary>
    [System.Flags]
    public enum Filters
    {
        None = 0,
        HideLogs = 1,
        HideWarnings = 2,
        HideErrors = 4
    }

    /// <summary>
    /// Gets or sets the current filters being applied to this channel. Messages that match the specified set of flags will be ignored.
    /// </summary>
    public Filters Filter { get; set; }

    [StackTraceIgnore]
    public void Log(string message, params object[] par)
    {
        if ((Filter & Filters.HideLogs) != Filters.HideLogs)
        {
            UberDebug.LogChannel(_channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void Log(Object context, string message, params object[] par)
    {
        if ((Filter & Filters.HideLogs) != Filters.HideLogs)
        {
            UberDebug.LogChannel(context, _channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogWarning(string message, params object[] par)
    {
        if ((Filter & Filters.HideWarnings) != Filters.HideWarnings)
        {
            UberDebug.LogWarningChannel(_channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogWarning(Object context, string message, params object[] par)
    {
        if ((Filter & Filters.HideWarnings) != Filters.HideWarnings)
        {
            UberDebug.LogWarningChannel(context, _channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogError(string message, params object[] par)
    {
        if ((Filter & Filters.HideErrors) != Filters.HideErrors)
        {
            UberDebug.LogErrorChannel(_channelName, message, par);
        }
    }

    [StackTraceIgnore]
    public void LogError(Object context, string message, params object[] par)
    {
        if ((Filter & Filters.HideErrors) != Filters.HideErrors)
        {
            UberDebug.LogErrorChannel(context, _channelName, message, par);
        }
    }
}
