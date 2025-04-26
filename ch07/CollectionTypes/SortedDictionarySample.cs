namespace CollectionTypes;

public class SortedDictionarySample : IShowTitle
{
    public static void Run()
    {
        IShowTitle.ShowTitle(nameof(SortedDictionarySample));
        RacerId mc81 = new("mc", 81);
        RacerId mc4 = new("mc", 4);
        RacerId rb1 = new("rb", 1);
        RacerId rb16 = new("rb", 16);
        RacerId f47 = new("f", 47);
        RacerId f44 = new("f", 44);
        SortedDictionary<RacerId, Racer> racers = new()
        {
            { mc81, new Racer("Oscar", "Piastri", mc81) },
            { mc4, new Racer("Lando", "Norris", mc4) },
            { rb1, new Racer("Max", "Verstappen", rb1) },
            { rb16, new Racer("Yuki", "Tsunoda", rb16) },
            { f47, new Racer("Charles", "Leclerc", f47) },
            { f44, new Racer("Lewis", "Hamilton", f44) }
        };

        Console.WriteLine("enter a key to find a racer or press Enter to exit");
        string? input = Console.ReadLine();
        while (!string.IsNullOrEmpty(input))
        {
            if (!RacerId.TryParse(input, null, out RacerId racerId))
            {
                Console.WriteLine($"Invalid RacerId: {input}");
                input = Console.ReadLine();
                continue;
            }

            if (racers.TryGetValue(racerId, out Racer? racer))
            {
                Console.WriteLine($"Racer found: {racer.FirstName} {racer.LastName}");
            }
            else
            {
                Console.WriteLine($"Racer with ID {racerId} not found.");
            }
            Console.WriteLine("enter a key to find a racer or press Enter to exit");
            input = Console.ReadLine();
        }
        Console.WriteLine();
    }
}
