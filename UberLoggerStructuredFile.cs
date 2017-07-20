﻿using System;
using System.IO;
using UberLogger;
using UnityEngine;

/// <summary>
/// <para>File logger backend, which writes log files with a structured format:</para>
/// <para>[Timestamp, in UTC time coordinates] message/warning/exception file method message</para>
/// <para>Fields will be visually aligned via tabs.</para>
/// </summary>
public class UberLoggerStructuredFile : UberLogger.ILogger
{
    /// <summary>
    /// Which types of log messages shall include full callstacks
    /// </summary>
    public enum IncludeCallstackMode
    {
        Never,
        WarningsAndErrorsOnly,
        Always
    }

    /// <summary>
    /// What to do if the the log file already exists when UberLoggerStructuredFile starts
    /// </summary>
    public enum ExistingFileMode
    {
        /// <summary>
        /// Replace the existing log file with the new log file
        /// </summary>
        Overwrite,
        /// <summary>
        /// Add a suffix to the log file name, which makes the new file name unique
        /// </summary>
        DoNotOverwrite
    };

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
    /// Creates a logger which writes to disk with a structured format. Make sure to add the created object to UberLogger via Logger.AddLogger() as well.
    /// </summary>
    /// <param name="fileLogPath">Output file name (absolute path)</param>
    /// <param name="indentationSettings">Provide tab settings to get an output that uses tabs to align fields above each other visually. Pass null to always have 1 tab between columns.</param>
    /// <param name="includeCallStacks">When to show callstacks in log; never, only for warnings/errors, or always</param>
    /// <param name="existingFileMode">If set to Overwrite, overwrite existing log file; if set to DoNotOverwrite, add suffixes (.1 .2 etc) until an unused file name is found</param>
    public UberLoggerStructuredFile(string fileLogPath, IndentationSettings indentation, IncludeCallstackMode includeCallStacks = IncludeCallstackMode.WarningsAndErrorsOnly, ExistingFileMode existingFileHandling = ExistingFileMode.Overwrite)
    {
        IncludeCallStacks = includeCallStacks;
        Indentation = indentation;

        string fileLogPathWithCount = fileLogPath;
        if (existingFileHandling != ExistingFileMode.Overwrite)
        {
            // Look for the first nonexistent suitable file name 
            // Note, this poses a race condition if starting two clients on the same machine
            int suffixCounter = 0;
            while (System.IO.File.Exists(fileLogPathWithCount))
            {
                suffixCounter++;
                fileLogPathWithCount = fileLogPath + "." + suffixCounter;
            }
        }

        Debug.Log("Initialising file logging to " + fileLogPathWithCount);
        LogFileWriter = new StreamWriter(fileLogPathWithCount, false);
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
