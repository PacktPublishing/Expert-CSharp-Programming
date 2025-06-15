namespace AdvancedQuery;

internal static class StringExtensions
{
    extension (string name)
    {
        public string FirstName()
        {
            int ix = name.LastIndexOf(' ');
            return ix > 0 ? name[..ix] : name;
        }

        public string LastName()
        {
            int ix = name.LastIndexOf(' ');
            return ix >= 0 && ix < name.Length - 1 ? name[(ix + 1)..] : string.Empty;
        }
    }
}
