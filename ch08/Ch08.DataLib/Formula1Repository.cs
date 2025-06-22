using Microsoft.EntityFrameworkCore;

namespace Ch08.DataLib;

public class Formula1Repository : IFormula1Repository
{
    private readonly Formula1DataContext _context;
    private readonly SqlQueryLogger _sqlLogger;

    public Formula1Repository(Formula1DataContext context, SqlQueryLogger sqlLogger)
    {
        _context = context;
        _sqlLogger = sqlLogger;
    }

    public async Task<IEnumerable<Racer>> GetRacersAsync()
    {
        _sqlLogger.Clear();
        return await _context.Racers.Include(r => r.Team).ToListAsync();
    }

    public async Task<IEnumerable<Team>> GetTeamsAsync()
    {
        _sqlLogger.Clear();
        return await _context.Teams.Include(t => t.Racers).ToListAsync();
    }

    public async Task<IEnumerable<Racer>> GetRacersByCountryAsync(string country)
    {
        _sqlLogger.Clear();
        return await _context.Racers
            .Include(r => r.Team)
            .Where(r => r.Country == country)
            .ToListAsync();
    }

    public async Task<IEnumerable<Racer>> GetRacersWithMostWinsAsync(int minWins)
    {
        _sqlLogger.Clear();
        return await _context.Racers
            .Include(r => r.Team)
            .Where(r => r.NumberWins >= minWins)
            .OrderByDescending(r => r.NumberWins)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetRacersGroupedByCountryAsync()
    {
        _sqlLogger.Clear();
        return await _context.Racers
            .GroupBy(r => r.Country)
            .Select(g => new { Country = g.Key, Count = g.Count(), TotalWins = g.Sum(r => r.NumberWins) })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTeamsWithRacerCountAsync()
    {
        _sqlLogger.Clear();
        return await _context.Teams
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
        _sqlLogger.Clear();
        return await _context.Racers
            .Include(r => r.Team)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Team?> GetTeamByIdAsync(int id)
    {
        _sqlLogger.Clear();
        return await _context.Teams
            .Include(t => t.Racers)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public string GetLastExecutedSql()
    {
        return _sqlLogger.GetLastQuery();
    }
}