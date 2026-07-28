using System.Buffers;
using System.IO.Pipelines;
using System.Text;

Console.WriteLine("System.IO.Pipelines – High-Throughput Line Reading");
Console.WriteLine("--------------------------------------------------");

byte[] pipeData = Encoding.UTF8.GetBytes(
    "alpha\nbeta\ngamma\ndelta\nepsilon\n");

Pipe pipe = new();

// Writer task – writes data into the pipe
Task writeTask = Task.Run(async () =>
{
    PipeWriter pipeWriter = pipe.Writer;
    Memory<byte> buffer = pipeWriter.GetMemory(pipeData.Length);
    pipeData.CopyTo(buffer);
    pipeWriter.Advance(pipeData.Length);
    await pipeWriter.FlushAsync();
    await pipeWriter.CompleteAsync();
});

// Reader task – reads lines from the pipe
Task readTask = Task.Run(async () =>
{
    PipeReader pipeReader = pipe.Reader;
    int lineCount = 0;
    while (true)
    {
        ReadResult result = await pipeReader.ReadAsync();
        ReadOnlySequence<byte> buffer = result.Buffer;

        while (TryReadLine(ref buffer, out ReadOnlySequence<byte> lineSeq))
        {
            // Avoid ToArray() for the common single-segment case
            string text = lineSeq.IsSingleSegment
                ? Encoding.UTF8.GetString(lineSeq.FirstSpan)
                : Encoding.UTF8.GetString(lineSeq.ToArray());
            Console.WriteLine($"Reader: Line {++lineCount}: {text}");
        }

        pipeReader.AdvanceTo(buffer.Start, buffer.End);

        if (result.IsCompleted) break;
    }
    await pipeReader.CompleteAsync();
});

await Task.WhenAll(writeTask, readTask);
Console.WriteLine();

static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
{
    SequencePosition? position = buffer.PositionOf((byte)'\n');
    if (position is null) { line = default; return false; }

    line = buffer.Slice(0, position.Value);
    buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
    return true;
}
