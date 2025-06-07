namespace CustomImplementation;

internal static partial class CustomEnumerator
{
    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<T> Where1(Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        public IEnumerable<T> Where(Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
}
