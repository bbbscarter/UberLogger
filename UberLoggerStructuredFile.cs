using System;
using System.IO;
using UberLogger;
using UnityEngine;

/// <summary>
/// File logger backend, which writes log files with a structured format:
/// [Timestamp, in UTC time coordinates] <message/warning/exception> <file> <method> <message>
/// Fields will be visually aligned via tabs. If you 
/// </summary>
public class UberLoggerStructuredFile : UberLogger.ILogger
{
    public enum IncludeCallstackMode
    {
        Never,
        WarningsAndErrorsOnly,
        Always
    }

    private StreamWriter LogFileWriter;
    private IncludeCallstackMode IncludeCallStacks;

    [Serializable]
    public class IndentationSettings
    {
        /// <summary>
        /// Assumed tab size when VisualAlign is active
        /// </summary>
        public int TabSize = 8;

        // With (in tabs) of each column when VisualAlign is active
        public int TimeMinTabs = 4;
        public int MessageMinTabs = 16;
        public int ChannelMinTabs = 1;
        public int SeverityMinTabs = 2;
        public int FileNameMinTabs = 16;
        public int MethodMinTabs = 8;
    }

    public IndentationSettings Indentation;

    /// <summary>
    /// Constructor. Make sure to add it to UberLogger via Logger.AddLogger();
    /// <param name="fileLogPath">Output file name (absolute path)</param>
    /// <param name="indentationSettings">Provide tab settings to get an output that uses tabs to align fields above each other visually. Pass null to always have 1 tab between columns.</param>
    /// <param name="includeCallStacks">When to show callstacks in log; never, only for warnings/errors, or always</param>
    /// </summary>
    public UberLoggerStructuredFile(string fileLogPath, IndentationSettings indentation, IncludeCallstackMode includeCallStacks = IncludeCallstackMode.WarningsAndErrorsOnly)
    {
        IncludeCallStacks = includeCallStacks;
        Indentation = indentation;

        Debug.Log("Initialising file logging to " + fileLogPath);
        LogFileWriter = new StreamWriter(fileLogPath, false);
        LogFileWriter.AutoFlush = true;
    }

    /// <summary>
    /// Pad input string with at least one \t, but -- if the string is too short, and VisualAlign is requested, extra tabs
    /// </summary>
    private string PadString(string originalString, int minimumOutputTabs)
    {
        if (Indentation != null)
        {
            string str = originalString + "\t";
            int tabCount = ((str.Length + (Indentation.TabSize - 1)) / Indentation.TabSize);
            for (int i = tabCount; i < minimumOutputTabs; i++)
                str += "\t";

            return str;
        }
        else
            return originalString + "\t";
    }

    /// <summary>
    ///  Write one log entry to output file
    /// </summary>
    public void Log(LogInfo logInfo)
    {
        lock (this)
        {
            string formattedLine = "";

            // Timestamp

            string absoluteTimeStamp = logInfo.GetAbsoluteTimeStampAsString();
            formattedLine += PadString("[" + absoluteTimeStamp + "]", Indentation.TimeMinTabs);

            // Message

            string message = logInfo.Message;
            message = message.Replace(UberLogger.Logger.UnityInternalNewLine, " "); // Ensure message goes on a single line
            formattedLine += PadString(message, Indentation.MessageMinTabs);

            // Channel

            formattedLine += PadString(logInfo.Channel, Indentation.ChannelMinTabs);

            // Severity

            formattedLine += PadString(logInfo.Severity.ToString(), Indentation.SeverityMinTabs);

            // Source location + class/method

            if (logInfo.OriginatingSourceLocation != null)
            {
                LogStackFrame stackFrame = logInfo.OriginatingSourceLocation;

                formattedLine += PadString(stackFrame.GetFormattedFileName(), Indentation.FileNameMinTabs);

                formattedLine += PadString(stackFrame.GetFormattedMethodName(), Indentation.MethodMinTabs);
            }
            else
                formattedLine += PadString("<No callstack>", Indentation.FileNameMinTabs) + PadString("<Unknown method>", Indentation.MethodMinTabs);


            LogFileWriter.WriteLine(formattedLine);


            // Write callstack, if necessary

            bool includeCallStack = (logInfo.Callstack.Count > 0);

            if (IncludeCallStacks == IncludeCallstackMode.Never)
                includeCallStack = false;

            if (IncludeCallStacks == IncludeCallstackMode.WarningsAndErrorsOnly
                && logInfo.Severity == LogSeverity.Message)
                includeCallStack = false;

            if (includeCallStack)
            {
                foreach (var frame in logInfo.Callstack)
                {
                    string line = PadString("", Indentation.TimeMinTabs)
                        + PadString("", Indentation.MessageMinTabs)
                        + PadString(logInfo.Channel, Indentation.ChannelMinTabs)
                        + PadString("Callstack", Indentation.SeverityMinTabs)
                        + PadString(frame.GetFormattedFileName(), Indentation.FileNameMinTabs)
                        + PadString(frame.GetFormattedMethodName(), Indentation.MethodMinTabs);

                    LogFileWriter.WriteLine(line);
                }
                LogFileWriter.WriteLine();
            }
        }
    }
}
