using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestUberLogger : MonoBehaviour
{
    public Color TestColour;
    // Use this for initialization
    void Start ()
    {
        UberLogger.Logger.AddLogger(new UberLoggerFile("UberLogger.log"), false);
        DoTest();
        var t = new List<int>();
        t[0] = 5;

    }

    public void DoTest()
    {
        // UnityEngine.Debug.Log("Starting");
        Debug.LogWarning(gameObject, "Starting");
        Debug.LogError(gameObject, "Starting2");
        Debug.Log(gameObject, "Starting2");
        Debug.LogChannel("Test", "Test Channels");
        Debug.LogChannel("Test", gameObject, "Test Channels with GameObject");
        Debug.LogChannel("Test", gameObject, "Test Channels with GameObject and param {0}", 27);
        Debug.Log("Test param {0}", 27);
	
    }
	
	// Update is called once per frame
    void Update () {
        DoTest();
    }
}

