namespace CustomImplementation;

internal static partial class CustomEnumerator
{
    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<T> Filter1(Func<T, bool> predicate)
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

        public IEnumerable<T> Filter2(Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(predicate);

            return Iterator();

            IEnumerable<T> Iterator()
            {
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
}
