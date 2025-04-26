namespace Sorting;
internal class Formula1
{
    private readonly static IEnumerable<Racer> _racers = [
            new Racer("Oscar", "Piastri"),
            new Racer("Lando", "Norris"),
            new Racer("Max", "Verstappen"),
            new Racer("Yuki", "Tsunoda"),
            new Racer("Charles", "Leclerc"),
            new Racer("Lewis", "Hamilton"),
            new Racer("George", "Russell"),
            new Racer("Andrea Kimi", "Antonelli"),
            new Racer("Fernando", "Alonso"),
            new Racer("Lance", "Stroll"),
            new Racer("Pierre", "Gasly"),
            new Racer("Jack", "Doohan"),
            new Racer("Esteban", "Ocon"),
            new Racer("Oliver", "Bearman"),
            new Racer("Liam", "Lawson"),
            new Racer("Isack", "Hadjar"),
            new Racer("Carlos", "Sainz"),
            new Racer("Alexander", "Albon"),
            new Racer("Nico", "Hülkenberg"),
            new Racer("Gabriel", "Bortoleto")
        ];
    public static IEnumerable<Racer> GetRacers() => _racers;
}
