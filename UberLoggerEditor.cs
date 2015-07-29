#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UberLogger;


[System.Serializable]
public class UberLoggerEditor : ScriptableObject, ILogger
{
    public List<LogInfo> LogInfo = new List<LogInfo>();
    public bool PauseOnError = false;
    public bool ClearOnPlay = true;
    public bool WasPlaying = false;

    static public UberLoggerEditor Create()
    {
        var editorDebug = ScriptableObject.FindObjectOfType<UberLoggerEditor>();
        // UnityEngine.Debug.Log("Found " + editorDebug);

        if(editorDebug==null)
        {
            // UnityEngine.Debug.LogError("Creating new editor logger");
            editorDebug = ScriptableObject.CreateInstance<UberLoggerEditor>();
        }
        // else
        // {
        //     UnityEngine.Debug.Log("Found editor logger from searching");
        // }
        editorDebug.NoErrors = 0;
        editorDebug.NoWarnings = 0;
        editorDebug.NoMessages = 0;
        
        return editorDebug;
    }

    public void OnEnable()
    {
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
        //Make this scriptable object persist between Play sessions
        hideFlags = HideFlags.HideAndDontSave;
    }

    // public void OnDestroy()
    // {
    //     EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
    // }

    public void ProcessOnStartClear()
    {
        if(!WasPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if(ClearOnPlay)
            {
                Clear();
            }
        }
        WasPlaying = EditorApplication.isPlayingOrWillChangePlaymode;
    }
    
    void OnPlaymodeStateChanged()
    {
        ProcessOnStartClear();
    }

    public int NoErrors;
    public int NoWarnings;
    public int NoMessages;

    public void Log(LogInfo logInfo)
    {
        // UnityEngine.Debug.Break();
        // UnityEngine.Debug.Log("Getting log from " + logInfo.Message);
        ProcessOnStartClear();
        LogInfo.Add(logInfo);
        if(logInfo.Severity==LogSeverity.Error)
        {
            NoErrors++;
        }
        else if(logInfo.Severity==LogSeverity.Warning)
        {
            NoWarnings++;
        }
        else
        {
            NoMessages++;
        }
        if(logInfo.Severity==LogSeverity.Error && PauseOnError)
        {
            UnityEngine.Debug.Break();
        }
    }

    public void Clear()
    {
        LogInfo.Clear();
        NoWarnings = 0;
        NoErrors = 0;
        NoMessages = 0;
    }
}

#endif