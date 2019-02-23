# Unity-Windows-Console-For-Servers
Creates a window console.

##Write##
UnityConsole.WriteLine("Hello World !", ConsoleColor.Cyan);


##Read##
string output;
if (UnityConsole.TryReadLine(out output))
    Debug.Log(output);

thats all ^^
