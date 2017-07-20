﻿using UnityEngine;
using UberLogger;

/// <summary>
/// Place this component in the scene to log all console output to a file with a structured format
/// </summary>
public class UberLoggerLogToFile : MonoBehaviour
{
    public string OutputFile = "output_log_structured.txt";

    public UberLoggerStructuredFile.IndentationSettings Indentation;
    public UberLoggerStructuredFile.IncludeCallstackMode IncludeCallStacks;
    public UberLoggerStructuredFile.ExistingFileMode ExistingFile;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Create a logger that writes to persistentDataPath
        {
            UberLoggerStructuredFile uberLoggerFile = new UberLoggerStructuredFile(System.IO.Path.Combine(Application.persistentDataPath, OutputFile), Indentation, IncludeCallStacks, ExistingFile);
            UberLogger.Logger.AddLogger(uberLoggerFile);
        }

#if !UNITY_EDITOR
        // If running a separately-built executable, also create a logger that writes to dataPath
        // The file location is easy for people to find, but sometimes the location is read-only
        {
            UberLoggerStructuredFile uberLoggerFile = new UberLoggerStructuredFile(System.IO.Path.Combine(Application.dataPath, OutputFile), Indentation, IncludeCallStacks, ExistingFile);
            UberLogger.Logger.AddLogger(uberLoggerFile);
        }
#endif
    }
}