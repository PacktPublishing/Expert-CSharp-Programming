using static TrafficLightSample.TrafficLightColor;

namespace TrafficLightSample;

public static class TrafficLightWithTuplesAndCount
{
    public static void Run()
    {
        Console.WriteLine("Traffic light with state");
        TrafficLightColor currentLight = Red;
        TrafficLightColor previousLight = Amber;
        for (int i = 0; i < 30; i++)
        {
            Console.WriteLine($"current: {currentLight}, previous: {previousLight}");
            (currentLight, previousLight) = NextLight(currentLight, previousLight);
        }
        Console.WriteLine();
    }

    private static int _blinkCount = 0;
    private static (TrafficLightColor Current, TrafficLightColor Previous) NextLight(
        TrafficLightColor current, 
        TrafficLightColor previous) => 
        (current, previous) switch
    {
        (Red, _) => (Amber, current),
        (Amber, Red) => (Green, current),
        (Green, _) => (GreenBlinking, current),
        (GreenBlinking, _) => ++_blinkCount % 3 == 0 ? (Amber, current) : (GreenBlinking, current),
        (Amber, GreenBlinking) => (Red, current),
        _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
    };
}
