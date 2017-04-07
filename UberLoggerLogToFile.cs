using UnityEngine;
using UberLogger;

/// <summary>
/// Place this component in the scene to log all console output to a file with a structured format
/// </summary>
public class UberLoggerLogToFile : MonoBehaviour
{
    public string OutputFile = "output_log_structured.txt";

    public UberLoggerStructuredFile.IndentationSettings Indentation;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        UberLoggerStructuredFile uberLoggerFile = new UberLoggerStructuredFile(OutputFile, Indentation);
        UberLogger.Logger.AddLogger(uberLoggerFile);
    }
}