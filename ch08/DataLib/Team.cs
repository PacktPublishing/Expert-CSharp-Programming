namespace DataLib;
public record class Team(string Name, string Country, params IEnumerable<int> Championships);
