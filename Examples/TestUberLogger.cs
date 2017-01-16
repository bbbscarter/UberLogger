using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class TestUberLogger : MonoBehaviour
{
    Thread TestThread;
    // Use this for initialization
    void Start ()
    {
        UberLogger.Logger.AddLogger(new UberLoggerFile("UberLogger.log"), false);
        DoTest();
        TestThread = new Thread(new ThreadStart(TestThreadEntry));
        TestThread.Start();

        //Test an internal .Net OOB error
        var t = new List<int>();
        t[0] = 5;
    }

    void OnDestroy()
    {
        TestThread.Abort();
        TestThread.Join();
    }
    void TestThreadEntry()
    {
        for(;;)
        {
            Debug.Log("Thread Log Message");
            UberDebug.Log("Thread ULog Message");
            Thread.Sleep(100);
        }
    }

    public void DoTest()
    {
        // UnityEngine.Debug.Log("Starting");
        Debug.LogWarning("Log Warning with GameObject", gameObject);
        Debug.LogError("Log Error with GameObject", gameObject);
        Debug.Log("Log Message with GameObject", gameObject);
        Debug.LogFormat("Log Format param {0}", "Test");
        Debug.LogFormat(gameObject, "Log Format with GameObject and param {0}", "Test");

        UberDebug.Log("ULog");
        UberDebug.Log("ULog with param {0}", "Test");
        UberDebug.Log(gameObject, "ULog with GameObject");
        UberDebug.Log(gameObject, "ULog with GameObject and param {0}", "Test");

        UberDebug.LogChannel("Test", "ULogChannel");
        UberDebug.LogChannel("Test", "ULogChannel with param {0}", "Test");
        UberDebug.LogChannel(gameObject, "Test", "ULogChannel with GameObject");
        UberDebug.LogChannel(gameObject, "Test", "ULogChannel with GameObject and param {0}", "Test");
	
        UberDebug.LogWarning("ULogWarning");
        UberDebug.LogWarning("ULogWarning with param {0}", "Test");
        UberDebug.LogWarning(gameObject, "ULogWarning with GameObject");
        UberDebug.LogWarning(gameObject, "ULogWarning with GameObject and param {0}", "Test");

        UberDebug.LogWarningChannel("Test", "ULogWarningChannel");
        UberDebug.LogWarningChannel("Test", "ULogWarningChannel with param {0}", "Test");
        UberDebug.LogWarningChannel(gameObject, "Test", "ULogWarningChannel with GameObject");
        UberDebug.LogWarningChannel(gameObject, "Test", "ULogWarningChannel with GameObject and param {0}", "Test");

        UberDebug.LogError("ULogError");
        UberDebug.LogError("ULogError with param {0}", "Test");
        UberDebug.LogError(gameObject, "ULogError with GameObject");
        UberDebug.LogError(gameObject, "ULogError with GameObject and param {0}", "Test");

        UberDebug.LogErrorChannel("Test", "ULogErrorChannel");
        UberDebug.LogErrorChannel("Test", "ULogErrorChannel with param {0}", "Test");
        UberDebug.LogErrorChannel(gameObject, "Test", "ULogErrorChannel with GameObject");
        UberDebug.LogErrorChannel(gameObject, "Test", "ULogErrorChannel with GameObject and param {0}", "Test");
    }
	
	// Update is called once per frame
    void Update ()
    {
        // DoTest();
    }
}

