namespace LiveShaping;

public class Formula1
{
    private List<Racer>? _racers;
    public IEnumerable<Racer> Racers => _racers ??= GetRacers();

    private List<Racer> GetRacers() => [
            new() { Name = "Sebastian Vettel", Team = "Red Bull Racing", Number = 1 },
            new() { Name = "Mark Webber", Team = "Red Bull Racing", Number = 2 },
            new() { Name = "Jenson Button", Team = "McLaren", Number = 3 },
            new() { Name = "Lewis Hamilton", Team = "McLaren", Number = 4 },
            new() { Name = "Fernando Alonso", Team = "Ferrari", Number = 5 },
            new() { Name = "Felipe Massa", Team = "Ferrari", Number = 6 },
            new() { Name = "Michael Schumacher", Team = "Mercedes", Number = 7 },
            new() { Name = "Nico Rosberg", Team = "Mercedes", Number = 8 },
            new() { Name = "Kimi Raikkonen", Team = "Lotus", Number = 9 },
            new() { Name = "Romain Grosjean", Team = "Lotus", Number = 10 },
            new() { Name = "Paul di Resta", Team = "Force India", Number = 11 },
            new() { Name = "Nico Hülkenberg", Team = "Force India", Number = 12 },
            new() { Name = "Kamui Kobayashi", Team = "Sauber", Number = 14 },
            new() { Name = "Sergio Perez", Team = "Sauber", Number = 15 },
            new() { Name = "Daniel Ricciardo", Team = "Toro Rosso", Number = 16 },
            new() { Name = "Jean-Eric Vergne", Team = "Toro Rosso", Number = 17 },
            new() { Name = "Pastor Maldonado", Team = "Williams", Number = 18 },
            new() { Name = "Bruno Senna", Team = "Williams", Number = 19 },
            new() { Name = "Heikki Kovalainen", Team = "Caterham", Number = 20 },
            new() { Name = "Witali Petrow", Team = "Caterham", Number = 21 },
            new() { Name = "Pedro de la Rosa", Team = "HRT", Number = 22 },
            new() { Name = "Narain Karthikeyan", Team = "HRT", Number = 23 },
            new() { Name = "Timo Glock", Team = "Marussia", Number = 24 },
            new() { Name = "Charles Pic", Team = "Marussia", Number = 25 }
          ];
}
