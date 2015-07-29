using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UberLogger
{
    [AttributeUsage(AttributeTargets.Method)]
    public class StackTraceIgnore : Attribute {}

    [AttributeUsage(AttributeTargets.Method)]
    public class LogUnityOnly : Attribute {}

    public enum LogSeverity
    {
        Message,
        Warning,
        Error,
    }

    [System.Serializable]
    public class LogStackFrame
    {
        public string MethodName;
        public string DeclaringType;
        public string ParameterSig;

        public int LineNumber;
        public string FileName;

        string FormattedMethodName;

        public LogStackFrame(StackFrame frame)
        {
            var method = frame.GetMethod();
            MethodName = method.Name;
            DeclaringType = method.DeclaringType.Name;

            var pars = method.GetParameters();
            for (int c1=0; c1<pars.Length; c1++)
            {
                ParameterSig += String.Format("{0} {1}", pars[c1].ParameterType, pars[c1].Name);
                if(c1+1<pars.Length)
                {
                    ParameterSig += ", ";
                }
            }

            FileName = frame.GetFileName();
            LineNumber = frame.GetFileLineNumber();
            FormattedMethodName = MakeFormattedMethodName();
        }

        public LogStackFrame(string unityStackFrame)
        {
            if(Logger.ExtractInfoFromUnityStackInfo(unityStackFrame, ref DeclaringType, ref MethodName, ref FileName, ref LineNumber))
            {
                FormattedMethodName = MakeFormattedMethodName();
            }
            else
            {
                FormattedMethodName = unityStackFrame;
            }
        }


        public LogStackFrame(string message, string filename, int lineNumber)
        {
            FileName = filename;
            LineNumber = lineNumber;
            FormattedMethodName = message;
        }



        public string GetFormattedMethodName()
        {
            return FormattedMethodName;
        }

        string MakeFormattedMethodName()
        {
            string filename = FileName;
            if(!String.IsNullOrEmpty(FileName))
            {
                var startSubName = FileName.IndexOf("Assets", StringComparison.OrdinalIgnoreCase);

                if(startSubName>0)
                {
                    filename = FileName.Substring(startSubName);
                }
            }
            string methodName = String.Format("{0}.{1}({2}) (at {3}:{4})", DeclaringType, MethodName, ParameterSig, filename, LineNumber);
            return methodName;
        }
    }

    [System.Serializable]
    public class LogInfo
    {
        public GameObject Source;
        public string Channel;
        public LogSeverity Severity;
        public string Message;
        public List<LogStackFrame> Callstack;
        public double TimeStamp;
        string TimeStampAsString;

        public string GetTimeStampAsString()
        {
            return TimeStampAsString;
        }

        public LogInfo(GameObject source, string channel, LogSeverity severity, List<LogStackFrame> callstack, object message, params object[] par)
        {
            Source = source;
            Channel = channel;
            Severity = severity;
            var messageString = message as String;
            if(messageString!=null)
            {
                Message = System.String.Format(messageString, par);
            }
            else
            {
                Message = message.ToString();
            }

            Callstack = callstack;
            TimeStamp = Logger.GetTime();
            TimeStampAsString = String.Format("{0:0.0000}", TimeStamp);
        }
    }

    public static class Logger
    {
        static List<ILogger> Loggers = new List<ILogger>();
        static LinkedList<LogInfo> RecentMessages = new LinkedList<LogInfo>();
        public static int MaxMessagesToKeep = 1000;
        static double StartTime;

        static bool AlreadyLogging = false;

        static Logger()
        {
            // Application.RegisterLogCallback(UnityLogHandler);
            Application.logMessageReceived += UnityLogHandler;
            StartTime = GetTime();
        }

        [StackTraceIgnore]
        static void UnityLogHandler(string logString, string stackTrace, UnityEngine.LogType logType)
        {
            UnityLogInternal(logString, stackTrace, logType);
        }
    
        static public double GetTime()
        {
#if UNITY_EDITOR
            return EditorApplication.timeSinceStartup - StartTime;
#else
            double time = Time.time;
            return time - StartTime;
#endif
        }

        static public void AddLogger(ILogger logger, bool populateWithExistingMessages=true)
        {
            lock(Loggers)
            {
                if(populateWithExistingMessages)
                {
                    foreach(var oldLog in RecentMessages)
                    {
                        logger.Log(oldLog);
                    }
                }

                if(!Loggers.Contains(logger))
                {
                    Loggers.Add(logger);
                }
            }
        }

        static public bool ExtractInfoFromUnityMessage(string log, ref string filename, ref int lineNumber)
        {
            // log = "Assets/Code/Debug.cs(140,21): warning CS0618: 'some error'
            var match = System.Text.RegularExpressions.Regex.Matches(log, @"(.*)\((\d+).*\)");

            if(match.Count>0)
            {
                filename = match[0].Groups[1].Value;
                lineNumber = Convert.ToInt32(match[0].Groups[2].Value);
                return true;
            }
            return false;
        }
    

        static public bool ExtractInfoFromUnityStackInfo(string log, ref string declaringType, ref string methodName, ref string filename, ref int lineNumber)
        {
            // log = "DebugLoggerEditorWindow.DrawLogDetails () (at Assets/Code/Editor.cs:298)";
            //Test.Start () 
            var match = System.Text.RegularExpressions.Regex.Matches(log, @"(.*)\.(.*)\s*\(.*\(at (.*):(\d+)");

            if(match.Count>0)
            {
                declaringType = match[0].Groups[1].Value;
                methodName = match[0].Groups[2].Value;
                filename = match[0].Groups[3].Value;
                lineNumber = Convert.ToInt32(match[0].Groups[4].Value);
                return true;
            }
            return false;
        }

        [StackTraceIgnore]
        // Returns false if the stack frame contains any methods flagged as LogUnityOnly
        static bool GetCallstack(ref List<LogStackFrame> callstack)
        {
            callstack.Clear();
            StackTrace stackTrace = new StackTrace(true);           // get call stack
            StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

            // write call stack method names
            foreach (StackFrame stackFrame in stackFrames)
            {
                var method = stackFrame.GetMethod();
                if(method.IsDefined(typeof(LogUnityOnly), true))
                {
                    return true;
                }
                if(!method.IsDefined(typeof(StackTraceIgnore), true))
                {
                    if(!(method.Name=="CallLogCallback" && method.DeclaringType.Name=="Application")
                       && !(method.DeclaringType.Name=="Debug" && (method.Name=="Internal_Log" || method.Name=="Log")))
                    {
                        var logStackFrame = new LogStackFrame(stackFrame);
                        
                        callstack.Add(logStackFrame);
                        
                    }
                }
            } 
        
            return false;
        }

        static List<LogStackFrame> GetCallstackFromUnityLog(string unityCallstack)
        {
            var lines = System.Text.RegularExpressions.Regex.Split(unityCallstack, System.Environment.NewLine); 
            var stack = new List<LogStackFrame>();
            foreach(var line in lines)
            {
                var frame = new LogStackFrame(line);
                if(!string.IsNullOrEmpty(frame.GetFormattedMethodName()))
                {
                    stack.Add(new LogStackFrame(line));
                }
            }
            return stack;
        }

        [StackTraceIgnore()]
        static void UnityLogInternal(string unityMessage, string unityCallStack, UnityEngine.LogType logType)
        {
            lock(Loggers)
            {
                if(!AlreadyLogging)
                {
                    try
                    {
                        AlreadyLogging = true;
                    
                        var callstack = new List<LogStackFrame>();
                        var unityOnly = GetCallstack(ref callstack);
                        if(unityOnly)
                        {
                            return;
                        }

                        if(callstack.Count==0)
                        {
                            callstack = GetCallstackFromUnityLog(unityCallStack);
                        }

                        LogSeverity severity;
                        switch(logType)
                        {
                            case UnityEngine.LogType.Error: severity = LogSeverity.Error; break;
                            case UnityEngine.LogType.Exception: severity = LogSeverity.Error; break;
                            case UnityEngine.LogType.Warning: severity = LogSeverity.Warning; break;
                            default: severity = LogSeverity.Message; break;
                        }

                        string filename = "";
                        int lineNumber = 0;
                    
                        if(ExtractInfoFromUnityMessage(unityMessage, ref filename, ref lineNumber))
                        {
                            callstack.Insert(0, new LogStackFrame(unityMessage, filename, lineNumber));
                        }

                        var logInfo = new LogInfo(null, "", severity, callstack, unityMessage);

                        RecentMessages.AddLast(logInfo);
                        TrimOldMessages();

                        Loggers.RemoveAll(l=>l==null);
                        Loggers.ForEach(l=>l.Log(logInfo));
                    }
                    finally
                    {
                        AlreadyLogging = false;
                    }
                }
            }
        }


        [StackTraceIgnore()]
        static public void Log(string channel, GameObject source, LogSeverity severity, object message, params object[] par)
        {
            lock(Loggers)
            {
                if(!AlreadyLogging)
                {
                    try
                    {
                        AlreadyLogging = true;
                        var callstack = new List<LogStackFrame>();
                        var unityOnly = GetCallstack(ref callstack);
                        if(unityOnly)
                        {
                            return;
                        }

                        var logInfo = new LogInfo(source, channel, severity, callstack, message, par);

                        RecentMessages.AddLast(logInfo);
                        TrimOldMessages();
                        Loggers.RemoveAll(l=>l==null);
                        Loggers.ForEach(l=>l.Log(logInfo));
                    }
                    finally
                    {
                        AlreadyLogging = false;
                    }
                }
            }
        }
        static public T GetLogger<T>() where T:class
        {
            foreach(var logger in Loggers)
            {
                if(logger is T)
                {
                    return logger as T;
                }
            }
            return null;
        }

        static void TrimOldMessages()
        {
            while(RecentMessages.Count > MaxMessagesToKeep)
            {
                RecentMessages.RemoveFirst();
            }
        }
    }

    public interface ILogger
    {
        void Log(LogInfo logInfo);
    }
}
