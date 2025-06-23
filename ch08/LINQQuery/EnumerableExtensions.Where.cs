public static partial class EnumerableExtensions
{
    extension<T>(IEnumerable<T> source)
    {
        public IEnumerable<T> Where(Func<T, bool> predicate)
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
