namespace Ch08.DataLib;

public interface IFormula1Repository
{
    Task<(IEnumerable<Racer> Racers, int TotalCount)> GetRacersAsync(int skip = 0, int take = 10);
    Task<IEnumerable<string>> GetAllCountriesAsync();
    Task<IEnumerable<Team>> GetTeamsAsync();
    Task<IEnumerable<Racer>> GetRacersByCountryAsync(string country);
    Task<IEnumerable<Racer>> GetRacersWithMostWinsAsync(int minWins);
    Task<IEnumerable<object>> GetRacersGroupedByCountryAsync();
    Task<IEnumerable<object>> GetTeamsWithRacerCountAsync();
    Task<Racer?> GetRacerByIdAsync(int id);
    Task<Team?> GetTeamByIdAsync(int id);
    string GetLastExecutedSql();
}