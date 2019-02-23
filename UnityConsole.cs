using System;
using System.Text;
using UnityEngine;

public static class UnityConsole
{
    private static void SetTitle(string title)
    {
        Init();
        SetConsoleTitle(title);
    }

    public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
    {
        Init();
        Console.ForegroundColor = color;
        Console.WriteLine(text);
    }

    private static string ReadBuffer = "";
    public static bool TryReadLine(out string text)
    {
        Init();

        lock (ReadBuffer)
            if (ReadBuffer != string.Empty)
            {
                text = ReadBuffer;
                ReadBuffer = string.Empty;
                return true;
            }
        text = null;
        return false;
    }

    private static bool inited = false;
    private static void Init()
    {
        if (inited)
            return;
        inited = true;
        InitializeConsole();
    }
    private static System.IO.FileStream OutputWriter = null;
    private static System.IO.FileStream OutputReader = null;

    private static System.IO.TextReader old_In;
    private static System.IO.TextWriter old_Out;
    private static void InitializeConsole()
    {
        if (!AttachConsole(0x0ffffffff))
        {
            AllocConsole();
        }
        try
        {
            old_In = Console.In;
            old_Out = Console.Out;

            OutputWriter = new System.IO.FileStream(new Microsoft.Win32.SafeHandles.SafeFileHandle(GetStdHandle(STD_OUTPUT_HANDLE), true), System.IO.FileAccess.Write);
            OutputReader = new System.IO.FileStream(new Microsoft.Win32.SafeHandles.SafeFileHandle(GetStdHandle(STD_INPUT_HANDLE), true), System.IO.FileAccess.Read);

            System.IO.StreamWriter standardOutput = new System.IO.StreamWriter(OutputWriter, Encoding.UTF8);
            System.IO.StreamReader standardInput = new System.IO.StreamReader(OutputReader, Encoding.UTF8);

            standardOutput.AutoFlush = true;

            Console.SetOut(standardOutput);
            Console.SetIn(standardInput);

            Console.Clear();

            Application.quitting += Application_quitting;
            Application.logMessageReceivedThreaded += Application_logMessageReceived;

            System.Threading.Thread t = new System.Threading.Thread(AsyncRead);
            t.Priority = System.Threading.ThreadPriority.Lowest;
            t.Start();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("Couldn't redirect output: " + e.Message);
        }
    }

    private static void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            Application_quitting();
            return;
        }
#endif
        if (type == LogType.Log)
            WriteLine("UnityEngine : "+condition, ConsoleColor.White);
        else if (type == LogType.Error)
            WriteLine("UnityEngine : "+condition + "\n" + stackTrace, ConsoleColor.Red);
        else if (type == LogType.Exception)
            WriteLine("UnityEngine : "+condition + "\n" + stackTrace, ConsoleColor.Red);
    }

    private static void Application_quitting()
    {
        Shutdown();

        Application.logMessageReceivedThreaded -= Application_logMessageReceived;
        Application.quitting -= Application_quitting;
    }

    private static void Shutdown()
    {
        if (inited)
        {
            inited = false;
            FreeConsole();

            Console.In.Close();
            Console.Out.Close();

            Console.SetIn(old_In);
            Console.SetOut(old_Out);

            old_In = null;
            old_Out = null;
        }

        if (OutputWriter != null)
        {

            try
            {
                OutputWriter.Close();
            }
            catch { }
            OutputWriter = null;
        }

        if (OutputReader != null)
        {
            try
            {
                OutputReader.Close();
            }
            catch { }
            OutputReader = null;
        }
    }
    private static void AsyncRead()
    {
        try
        {
            while (inited)
            {
                string text = Console.ReadLine();

                lock (ReadBuffer)
                    ReadBuffer = text;

                while (ReadBuffer != string.Empty)
                    System.Threading.Thread.Sleep(25);
            }
        }
        catch { }
    }


    private const int STD_INPUT_HANDLE = -10;
    private const int STD_OUTPUT_HANDLE = -11;

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AttachConsole(uint dwProcessId);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    static extern bool AllocConsole();

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    static extern bool FreeConsole();

    [System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleTitle(string lpConsoleTitle);

}
