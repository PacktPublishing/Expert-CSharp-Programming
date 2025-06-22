namespace Ch08.DataLib;

public class Racer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public DateOnly BirthDay { get; set; }
    public int NumberWins { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }

    public override string ToString() => $"{FirstName} {LastName}";
}