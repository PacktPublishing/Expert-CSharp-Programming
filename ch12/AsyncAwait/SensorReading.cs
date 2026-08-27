namespace AsyncAwait;

/// <summary>Immutable sensor reading — positional record with primary constructor.</summary>
internal record struct SensorReading(int Id, double Temperature, DateTimeOffset Timestamp)
{
    public override readonly string ToString() =>
        $"Sensor {Id:D2}: {Temperature:F1}°C at {Timestamp:HH:mm:ss.fff}";
}
