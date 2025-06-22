using Microsoft.EntityFrameworkCore;

namespace Ch08.DataLib;

public class Formula1Repository(Formula1DataContext context, SqlQueryLogger sqlLogger) : IFormula1Repository
{
    public async Task<IEnumerable<Racer>> GetRacersAsync()
    {
        sqlLogger.Clear();
        return await context.Racers.Include(r => r.Team).ToListAsync();
    }

    public async Task<IEnumerable<Team>> GetTeamsAsync()
    {
        sqlLogger.Clear();
        return await context.Teams.Include(t => t.Racers).ToListAsync();
    }

    public async Task<IEnumerable<Racer>> GetRacersByCountryAsync(string country)
    {
        sqlLogger.Clear();
        return await context.Racers
            .Include(r => r.Team)
            .Where(r => r.Country == country)
            .ToListAsync();
    }

    public async Task<IEnumerable<Racer>> GetRacersWithMostWinsAsync(int minWins)
    {
        sqlLogger.Clear();
        return await context.Racers
            .Include(r => r.Team)
            .Where(r => r.NumberWins >= minWins)
            .OrderByDescending(r => r.NumberWins)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetRacersGroupedByCountryAsync()
    {
        sqlLogger.Clear();
        return await context.Racers
            .GroupBy(r => r.Country)
            .Select(g => new { Country = g.Key, Count = g.Count(), TotalWins = g.Sum(r => r.NumberWins) })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTeamsWithRacerCountAsync()
    {
        sqlLogger.Clear();
        return await context.Teams
            .Select(t => new { 
                TeamName = t.Name, 
                Country = t.Country,
                FoundedYear = t.FoundedYear,
                RacerCount = t.Racers.Count(),
                TotalWins = t.Racers.Sum(r => r.NumberWins)
            })
            .OrderByDescending(x => x.RacerCount)
            .ToListAsync();
    }

    public async Task<Racer?> GetRacerByIdAsync(int id)
    {
        sqlLogger.Clear();
        return await context.Racers
            .Include(r => r.Team)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Team?> GetTeamByIdAsync(int id)
    {
        sqlLogger.Clear();
        return await context.Teams
            .Include(t => t.Racers)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public string GetLastExecutedSql()
    {
        return sqlLogger.GetLastQuery();
    }
}