namespace Sorting;
internal class RacerComparer(RacerCompareType racerCompareType) : IComparer<Racer>
{
    public int Compare(Racer? x, Racer? y)
    {
        if (x is null && y is null) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return racerCompareType switch
        {
            RacerCompareType.ByFirstName => x.FirstName.CompareTo(y.FirstName)
                .ThenBy(() => x.LastName.CompareTo(y.LastName)),
            RacerCompareType.ByLastName => x.LastName.CompareTo(y.LastName)
                .ThenBy(() => x.FirstName.CompareTo(y.FirstName)),
            _ => throw new ArgumentOutOfRangeException(nameof(racerCompareType), racerCompareType, null)
        };
    }
}

public enum RacerCompareType
{
    ByFirstName,
    ByLastName,
}