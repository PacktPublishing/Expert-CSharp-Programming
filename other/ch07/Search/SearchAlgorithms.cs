using System.Diagnostics;

namespace Search;
internal static class SearchAlgorithms
{
    extension<T> (IList<T> source)
    {
        public T? LinearSearch(Func<T, bool> match)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (match(source[i]))
                {
                    return source[i];
                }
            }
            return default;
        }
    }

    extension<T>(IList<T> source)
        where T : IComparable<T>
    {
        public int BinarySearch1(T match)
        {
            int lo = 0;
            int hi = source.Count - 1;
            while (lo <= hi)
            {
                int i = lo + ((hi - lo) >> 1);
                int order = source[i].CompareTo(match);

                if (order == 0) return i;
                if (order < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            return ~lo;
        }
    }
}
