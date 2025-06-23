using System.Collections;

namespace CustomImplementation;

public partial class FilterEnumerable<T>(
  IEnumerable<T> source,
  Func<T, bool> predicate) :
    IEnumerable<T>
{
    public IEnumerator<T> GetEnumerator() =>
      new FilterEnumerator(
        source, predicate);

    IEnumerator IEnumerable.GetEnumerator() =>
      GetEnumerator();
}
