using static TrafficLightSample.TrafficLightColor;

namespace TrafficLightSample;

public static class TrafficLightWithTuples
{
    public static void Run()
    {
        Console.WriteLine("Using tuples");  
        TrafficLightColor currentLight = Red;
        TrafficLightColor previousLight = Amber;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"current: {currentLight}, previous: {previousLight}");
            (currentLight, previousLight) = NextLight(currentLight, previousLight);
        }
        Console.WriteLine();
    }

    private static (TrafficLightColor Current, TrafficLightColor Previous) NextLight(
        TrafficLightColor current, 
        TrafficLightColor previous) => 
        (current, previous) switch
    {
        (Red, _) => (Amber, current),
        (Amber, Red) => (Green, current),
        (Green, _) => (Amber, current),
        (Amber, Green) => (Red, current),
        _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
    };
}
