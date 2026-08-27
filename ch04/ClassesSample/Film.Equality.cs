namespace ClassesSample;

public partial class Film : IEquatable<Film>
{
    private const int HashMultiplier = unchecked((int)0xa5555529);  // a large prime number that provides good hash distribution, used for record types 

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = EqualityContract.GetHashCode() * HashMultiplier;
            hashCode = hashCode + Title.GetHashCode() * HashMultiplier;
            hashCode = hashCode + (Director?.GetHashCode() ?? 0) * HashMultiplier;
            hashCode = hashCode + Year.GetHashCode();
            return hashCode;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Film film && Equals(film) || base.Equals(obj);
    }

    protected virtual Type EqualityContract => typeof(Film);

    public bool Equals(Film? other)
    {
        if (ReferenceEquals(this, other))
            return true;

        if (other is null)
            return false;

        if (EqualityContract != other.EqualityContract)
            return false;

        if (Title != other.Title)
            return false;

        if (Director != other.Director)
            return false;

        return (Year != other.Year);
    }

    public static bool operator ==(Film? left, Film? right) => Equals(left, right);
    public static bool operator !=(Film? left, Film? right) => !Equals(left, right);
}
