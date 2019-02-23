# Unity-Windows-Console-For-Servers
Creates a window console.

<h2>Write To Console</h2>
<pre><code class='language-cs'>
UnityConsole.WriteLine("Hello World !", ConsoleColor.Cyan);
</code></pre>


<h2>Read From Console</h2>
string output;
if (UnityConsole.TryReadLine(out output))
    Debug.Log(output);


thats all ^^
