namespace AsyncAwait;

/// <summary>Parsed record returned from the parse step in the async pipeline.</summary>
internal record class ParsedRecord(string Source, int Length)
{
    public override string ToString() => $"ParsedRecord(source='{Source}', length={Length})";
}
