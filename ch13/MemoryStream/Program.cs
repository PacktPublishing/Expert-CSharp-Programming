using System.Text;

Console.WriteLine("MemoryStream – In-Memory Read/Write");
Console.WriteLine("-----------------------------------");

string originalText = "Streaming data efficiently is a core skill in modern .NET applications.";
byte[] textBytes = Encoding.UTF8.GetBytes(originalText);

using (MemoryStream ms = new(textBytes))
using (StreamReader sr = new(ms, Encoding.UTF8, leaveOpen: true))
{
    string content = await sr.ReadToEndAsync();
    Console.WriteLine($"Read  {ms.Length,4} bytes: \"{content[..42]}...\"");
}

// Write with a StreamWriter, then read back
using MemoryStream writeStream = new();
await using (StreamWriter streamWriter = new(writeStream, Encoding.UTF8, leaveOpen: true))
{
    await streamWriter.WriteLineAsync("Line 1: sensor_id=42, temp=21.5");
    await streamWriter.WriteLineAsync("Line 2: sensor_id=42, temp=22.1");
    await streamWriter.WriteLineAsync("Line 3: sensor_id=42, temp=21.8");
}

writeStream.Position = 0;
using StreamReader lineReader = new(writeStream, Encoding.UTF8);
string? line;
while ((line = await lineReader.ReadLineAsync()) is not null)
    Console.WriteLine($"  {line}");
Console.WriteLine();
