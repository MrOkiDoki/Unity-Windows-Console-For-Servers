# Unity-Windows-Console-For-Servers
Creates a window console.

<h2>Write To Console</h2>
```csharp
UnityConsole.WriteLine("Hello World !", ConsoleColor.Cyan);



<h2>Read To Console</h2>
```csharp
string output;
if (UnityConsole.TryReadLine(out output))
    Debug.Log(output);


thats all ^^
