using static TrafficLightSample.TrafficLightColor;

namespace TrafficLightSample;

public static class TrafficLightWithType
{
    public static async Task RunAsync()
    {
        Console.WriteLine("Traffic light with type");
        TrafficLight currentLight = new(Red, Amber, TimeSpan.FromSeconds(1));
        for (int i = 0; i < 30; i++)
        {
            currentLight = NextLight(currentLight);
            Console.WriteLine($"current: {currentLight}");
            await Task.Delay(currentLight.Duration);
        }
        Console.WriteLine();
    }

    // The traffic light sequence is as follows:
    // Red -> Amber -> Green -> GreenBlinking -> Amber -> Red
    private static TrafficLight  NextLight(TrafficLight light)
    { 
        return light switch
        {
            { Current: Red } => light with { Current = Amber, Previous = light.Current, Duration = TimeSpan.FromSeconds(1) },
            { Current: Amber, Previous: Red } => light with { Current = Green, Previous = light.Current, Duration = TimeSpan.FromSeconds(3) },
            { Current: Green } => light with { Current = GreenBlinking, Previous = light.Current, Duration = TimeSpan.FromSeconds(0.5) },
            { Current: GreenBlinking } when light.BlinkCount % 3 == 0 => light with { Current = Amber, Previous = light.Current, Duration = TimeSpan.FromSeconds(1), BlinkCount = light.BlinkCount + 1 },
            { Current: GreenBlinking } => light with { BlinkCount = light.BlinkCount + 1 },
            { Current: Amber, Previous: GreenBlinking } => light with { Current = Red, Previous = light.Current, Duration = TimeSpan.FromSeconds(5) },
            _ => throw new ArgumentOutOfRangeException(nameof(light), light.Current, null)
        };
    }
}

public readonly record struct TrafficLight(TrafficLightColor Current, TrafficLightColor Previous, TimeSpan Duration, int BlinkCount = 1)
{
    public override readonly string ToString() => 
        $"{Current} previous: {Previous}, duration: {Duration.TotalSeconds}s, blink count: {BlinkCount}";
}
