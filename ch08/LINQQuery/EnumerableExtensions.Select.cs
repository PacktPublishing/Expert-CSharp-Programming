public static partial class EnumerableExtensions
 {
     extension<T>(IEnumerable<T> source)
     {
        public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(selector);

            return Iterator();

            IEnumerable<TResult> Iterator()
            {
                foreach (var item in source)
                {
                    yield return selector(item);
                }
            }
        }
    }
}
