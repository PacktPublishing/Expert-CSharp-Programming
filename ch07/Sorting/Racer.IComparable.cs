namespace Sorting;

public partial record class Racer : IComparable<Racer>
{
    public int CompareTo(Racer? other)
    {
        if (other is null) return 1;

        return RacerId.CompareTo(other.RacerId)
            .ThenBy(() => LastName.CompareTo(other.LastName))
            .ThenBy(() => FirstName.CompareTo(other.FirstName));
    }
}

public readonly partial record struct RacerId : IComparable<RacerId>
{
    public int CompareTo(RacerId other)
    {
        // alternative implementation using extension method
        //int id = Id;
        //return Prefix.CompareTo(other.Prefix)
        //    .ThenBy(() => id.CompareTo(other.Id));

        int result = Prefix.CompareTo(other.Prefix);
        if (result != 0) return result;
        return Id.CompareTo(other.Id);
    }
}
