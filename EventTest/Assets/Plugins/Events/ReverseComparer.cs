using System.Collections.Generic;

namespace KingDOM.Event
{
    /// <summary>
    /// Helper class, used for determining the right order of priority.
    /// </summary>
    /// <typeparam name="T">Type of values compared </typeparam>
    internal sealed class ReverseComparer<T> : IComparer<T> {
        private readonly IComparer<T> inner;
        internal ReverseComparer() : this(null) { }
        internal ReverseComparer(IComparer<T> inner)
        {
            this.inner = inner ?? Comparer<T>.Default;
        }
        int IComparer<T>.Compare(T x, T y) { return inner.Compare(y, x); }
    }
}