namespace Ch08.DataLib;

public interface IFormula1Repository
{
    Task<IEnumerable<Racer>> GetRacersAsync();
    Task<IEnumerable<Team>> GetTeamsAsync();
    Task<IEnumerable<Racer>> GetRacersByCountryAsync(string country);
    Task<IEnumerable<Racer>> GetRacersWithMostWinsAsync(int minWins);
    Task<IEnumerable<object>> GetRacersGroupedByCountryAsync();
    Task<IEnumerable<object>> GetTeamsWithRacerCountAsync();
    Task<Racer?> GetRacerByIdAsync(int id);
    Task<Team?> GetTeamByIdAsync(int id);
    string GetLastExecutedSql();
}