using Microsoft.EntityFrameworkCore;

namespace Ch08.DataLib;

public class Formula1Repository(IDbContextFactory<Formula1DataContext> contextFactory, SqlQueryLogger sqlLogger) : IFormula1Repository
{
    public async Task<(IEnumerable<Racer> Racers, int TotalCount)> GetRacersAsync(int skip = 0, int take = 10)
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        var query = context.Racers;

        var totalCount = await query.CountAsync();
        var racers = await query
            .OrderByDescending(r => r.Wins)
            .ThenBy(r => r.LastName)
            .Skip(skip)
            .Take(take)
            .TagWith(nameof(GetRacersAsync))
            .ToListAsync();

        return (racers, totalCount);
    }

    public async Task<IEnumerable<string>> GetAllCountriesAsync()
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Racers
            .Select(r => r.Country)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
    }

    public async Task<IEnumerable<Team>> GetTeamsAsync()
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Teams
            .Include(t => t.Racers.OrderBy(rt => rt.Year))
            .ThenInclude(rt => rt.Racer)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Racer>> GetRacersByCountryAsync(string country)
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Racers
            .Include(r => r.Teams.OrderBy(rt => rt.Year))
            .ThenInclude(rt => rt.Team)
            .Where(r => r.Country == country)
            .OrderBy(r => r.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Racer>> GetRacersWithMostWinsAsync(int minWins)
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Racers
            .Where(r => r.Wins >= minWins)
            .OrderByDescending(r => r.Wins)
            .TagWith(nameof(GetRacersWithMostWinsAsync))
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetRacersGroupedByCountryAsync()
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Racers
            .GroupBy(r => r.Country)
            .Select(g => new { 
                Country = g.Key, 
                Count = g.Count(), 
                TotalWins = g.Sum(r => r.Wins) 
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetTeamsWithRacerCountAsync()
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Teams
            .Select(t => new { 
                TeamName = t.Name, 
                Country = t.Country,
                FoundedYear = t.FoundedYear,
                RacerCount = t.Racers.Count(),
                TotalWins = t.Racers.Sum(rt => rt.Racer.Wins)
            })
            .OrderByDescending(x => x.RacerCount)
            .ToListAsync();
    }

    public async Task<Racer?> GetRacerByIdAsync(int id)
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Racers
            .Include(r => r.Teams.OrderBy(rt => rt.Year))
            .ThenInclude(rt => rt.Team)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Team?> GetTeamByIdAsync(int id)
    {
        sqlLogger.Clear();
        await using var context = await contextFactory.CreateDbContextAsync();
        return await context.Teams
            .Include(t => t.Racers.OrderBy(rt => rt.Year))
            .ThenInclude(rt => rt.Racer)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public string GetLastExecutedSql()
    {
        return sqlLogger.GetLastQuery();
    }
}