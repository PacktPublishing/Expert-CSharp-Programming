namespace Data;

public partial record class Racer : IComparable<Racer>
{
    public int CompareTo(Racer? other)
    {
        if (other is null) return 1;

        return LastName.CompareTo(other.LastName)
            .ThenBy(() => FirstName.CompareTo(other.FirstName))
            .ThenBy(() => Country.CompareTo(other.Country))
            .ThenBy(() => BirthDay.CompareTo(other.BirthDay));
    }
}
