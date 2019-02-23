# Unity-Windows-Console-For-Servers
Creates a window console.

<h2>Write To Console</h2>
```c#
UnityConsole.WriteLine("Hello World !", ConsoleColor.Cyan);
```


<h2>Read To Console</h2>
```c#
string output;
if (UnityConsole.TryReadLine(out output))
    Debug.Log(output);
```

thats all ^^
