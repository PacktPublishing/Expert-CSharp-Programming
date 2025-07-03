namespace TrafficLightBlazor.Models;

public record struct TrafficLight(TrafficLightColor Current, TrafficLightColor Previous, TimeSpan Duration, int BlinkCount = 1)
{
    public override string ToString() => $"{Current} previous: {Previous}, duration: {Duration.TotalSeconds}s, blink count: {BlinkCount}";
}