using static TrafficLightSample.TrafficLightColor;

namespace TrafficLightSample;

public static class SimpleTrafficLight
{
    public static void Run()
    {
        Console.WriteLine("Simple light");
        TrafficLightColor currentLight = TrafficLightColor.Red;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"current: {currentLight}");
            currentLight = NextLight(currentLight);
        }
        Console.WriteLine();
    }

    private static TrafficLightColor NextLight(
        TrafficLightColor current) => 
        current switch
        {
            Red => RedAndAmber,
            RedAndAmber => Green,
            Green => Amber,
            Amber => Red,
            _ => throw new ArgumentOutOfRangeException(nameof(current), current, null)
        };
}
