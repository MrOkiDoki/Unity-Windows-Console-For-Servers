# Unity-Windows-Console-For-Servers
Creates a window console.

<h2>Write To Console</h2>
```cs
UnityConsole.WriteLine("Hello World !", ConsoleColor.Cyan);
```


<h2>Read From Console</h2>
string output;
if (UnityConsole.TryReadLine(out output))
    Debug.Log(output);


thats all ^^
