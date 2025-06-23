using TrafficLightBlazor.Models;
using static TrafficLightBlazor.Models.TrafficLightColor;

namespace TrafficLightBlazor.Services;

public class TrafficLightService
{
    // Slovenia sequence: Green -> Amber -> Red -> Red & Amber -> Green
    public static TrafficLight NextLightSlovenia(TrafficLight light)
    {
        return light switch
        {
            { Current: Green } => light with { Current = Amber, Previous = light.Current, Duration = TimeSpan.FromSeconds(2), BlinkCount = 1 },
            { Current: Amber, Previous: Green } => light with { Current = Red, Previous = light.Current, Duration = TimeSpan.FromSeconds(3), BlinkCount = 1 },
            { Current: Red } => light with { Current = RedAndAmber, Previous = light.Current, Duration = TimeSpan.FromSeconds(1), BlinkCount = 1 },
            { Current: RedAndAmber } => light with { Current = Green, Previous = light.Current, Duration = TimeSpan.FromSeconds(4), BlinkCount = 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(light), light.Current, null)
        };
    }

    // Austria sequence: Green -> GreenBlinking (3x) -> Amber -> Red -> Amber -> Green
    public static TrafficLight NextLightAustria(TrafficLight light)
    {
        return light switch
        {
            { Current: Green } => light with { Current = GreenBlinking, Previous = light.Current, Duration = TimeSpan.FromSeconds(1), BlinkCount = 1 },
            { Current: GreenBlinking, BlinkCount: < 3 } => light with { BlinkCount = light.BlinkCount + 1 },
            { Current: GreenBlinking, BlinkCount: 3 } => light with { Current = Amber, Previous = light.Current, Duration = TimeSpan.FromSeconds(2), BlinkCount = 1 },
            { Current: Amber, Previous: GreenBlinking } => light with { Current = Red, Previous = light.Current, Duration = TimeSpan.FromSeconds(3), BlinkCount = 1 },
            { Current: Red } => light with { Current = Amber, Previous = light.Current, Duration = TimeSpan.FromSeconds(1), BlinkCount = 1 },
            { Current: Amber, Previous: Red } => light with { Current = Green, Previous = light.Current, Duration = TimeSpan.FromSeconds(4), BlinkCount = 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(light), light.Current, null)
        };
    }

    public static TrafficLight NextLight(TrafficLight light, Country country)
    {
        return country switch
        {
            Country.Slovenia => NextLightSlovenia(light),
            Country.Austria => NextLightAustria(light),
            _ => throw new ArgumentOutOfRangeException(nameof(country), country, null)
        };
    }
}