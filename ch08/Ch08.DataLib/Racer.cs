namespace Ch08.DataLib;

public class Racer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Country { get; set; }
    public DateOnly BirthDay { get; set; }
    public int NumberWins { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }

    public override string ToString() => $"{FirstName} {LastName}";
}