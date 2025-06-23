using System.Collections;

namespace CustomImplementation;

public partial class FilterEnumerable<T>
{
    internal sealed class FilterEnumerator(
        IEnumerable<T> source,
        Func<T, bool> predicate) :
        IEnumerator<T>
    {
        private IEnumerator<T>? _enumerator;
        private int _state = 1;

        public T Current
        { get; private set; } = default!;
        object? IEnumerator.Current => Current;

        public void Dispose() => 
            _enumerator?.Dispose();

        public bool MoveNext()
        {
            switch (_state)
            {
                case 1:
                    _enumerator = source.GetEnumerator();
                    _state = 2;
                    goto case 2; // Fall through to the next case
                case 2:
                    try
                    {
                        while (_enumerator != null && _enumerator.MoveNext())
                        {
                            var item = _enumerator.Current;
                            if (predicate(item))
                            {
                                Current = item;
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        Dispose();
                        throw;
                    }
                    break;
            }

            Dispose();
            return false;
        }

        // Legacy from COM
        public void Reset() =>
            throw new NotSupportedException(); // Most implementations do not support Reset
    }
}
