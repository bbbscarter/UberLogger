using System.Collections.Generic;
using UberLogger;
using UnityEngine;


public class UberLoggerChannel
{
    private string ChannelName;
    public UberLoggerChannel(string channelName, List<IFilter> filters=null)
    {
        ChannelName = channelName;
        if(filters!=null)
        {
            foreach(var filter in filters)
            {
                UberLogger.Logger.AddChannelFilter(ChannelName, filter);
            }
        }
    }

    public void AddFilter(IFilter filter)
    {
        UberLogger.Logger.AddChannelFilter(ChannelName, filter);
    }

    public void RemoveFilter(IFilter filter)
    {
        UberLogger.Logger.RemoveChannelFilter(ChannelName, filter);
    }


    [StackTraceIgnore]
    public void Log(string message, params object[] par)
    {
        UberDebug.LogChannel(ChannelName, message, par);
    }

    [StackTraceIgnore]
    public void Log(Object context, string message, params object[] par)
    {
        UberDebug.LogChannel(context, ChannelName, message, par);
    }

    [StackTraceIgnore]
    public void LogWarning(string message, params object[] par)
    {
        UberDebug.LogWarningChannel(ChannelName, message, par);
    }

    [StackTraceIgnore]
    public void LogWarning(Object context, string message, params object[] par)
    {
        UberDebug.LogWarningChannel(context, ChannelName, message, par);
    }

    [StackTraceIgnore]
    public void LogError(string message, params object[] par)
    {
        UberDebug.LogErrorChannel(ChannelName, message, par);
    }

    [StackTraceIgnore]
    public void LogError(Object context, string message, params object[] par)
    {
        UberDebug.LogErrorChannel(context, ChannelName, message, par);
    }
}
