namespace Ch08.DataLib;
public class RacerTeamMap(int teamId, int racerId, int year)
{
    public int TeamId { get; internal set; } = teamId;
    public int RacerId { get; internal set; } = racerId;
    public int Year { get; internal set; } = year;
    public Team? Team { get; set; }
    public Racer? Racer { get; set; }
}
