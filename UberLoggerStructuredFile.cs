using UberLogger;
using System.IO;
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
    private bool VisualAlign;

    /// <summary>
    /// Assumed tab size when VisualAlign is active
    /// </summary>
    private const int TabSize = 8;

    // With (in tabs) of each column when VisualAlign is active
    private const int PadTimeTabs = 4;
    private const int PadSeverityTabs = 2;
    private const int PadFileNameTabs = 16;
    private const int PadMethodTabs = 8;

    /// <summary>
    /// Constructor. Make sure to add it to UberLogger via Logger.AddLogger();
    /// <param name="filename">Output file name. Relative to Application.persistentDataPath.</param>
    /// <param name="visualAlign">Set this to get an output that uses tabs to align fields above each other visually. Set to false to always have 1 tab between columns.</param>
    /// <param name="includeCallStacks">When to show callstacks in log; never, only for warnings/errors, or always</param>
    /// </summary>
    public UberLoggerStructuredFile(string filename, bool visualAlign = true, IncludeCallstackMode includeCallStacks = IncludeCallstackMode.WarningsAndErrorsOnly)
    {
        IncludeCallStacks = includeCallStacks;
        VisualAlign = visualAlign;
        var fileLogPath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        Debug.Log("Initialising file logging to " + fileLogPath);
        LogFileWriter = new StreamWriter(fileLogPath, false);
        LogFileWriter.AutoFlush = true;
    }

    /// <summary>
    /// Pad input string with at least one \t, but -- if the string is too short, and VisualAlign is requested, extra tabs
    /// </summary>
    private string PadString(string originalString, int minimumOutputTabs)
    {
        if (VisualAlign)
        {
            string str = originalString + "\t";
            int tabCount = ((str.Length + (TabSize - 1)) / TabSize);
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
            formattedLine += PadString("[" + absoluteTimeStamp + "]", PadTimeTabs);

            // Severity

            formattedLine += PadString(logInfo.Severity + ":", PadSeverityTabs);

            // Source location + class/method

            if (logInfo.OriginatingSourceLocation != null)
            {
                LogStackFrame stackFrame = logInfo.OriginatingSourceLocation;

                formattedLine += PadString(stackFrame.FileName + "(" + stackFrame.LineNumber + "):", PadFileNameTabs);

                formattedLine += PadString(stackFrame.DeclaringType + "." + stackFrame.MethodName + "():", PadMethodTabs);
            }
            else
                formattedLine += PadString("<No callstack>:", PadFileNameTabs) + PadString("<Unknown method>:", PadMethodTabs);

            // Message

            string message = logInfo.Message;
            message = message.Replace(UberLogger.Logger.UnityInternalNewLine, " "); // Ensure message goes on a single line

            formattedLine += message;

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
                    string line = PadString("", PadTimeTabs)
                        + PadString("Callstack:", PadSeverityTabs)
                        + PadString(frame.FileName + "(" + frame.LineNumber + "):", PadFileNameTabs)
                        + PadString(frame.DeclaringType + "." + frame.MethodName + "()", PadMethodTabs);

                    LogFileWriter.WriteLine(line);
                }
                LogFileWriter.WriteLine();
            }
        }
    }
}
