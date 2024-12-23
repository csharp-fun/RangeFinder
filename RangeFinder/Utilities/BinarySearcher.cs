namespace RangeFinder.Utilities;

/// <summary>
/// A general binary searcher for Collections of IComparable.
/// It allows duplicated elements (no de-dup) unlike SortedSet, which provides similar functionality GetViewBetween.
/// Speed: Queries will be done in O(logN + K) where N:elements length, K: results length.
/// RAM consumption: size of T * elements length will be used as all the elements will be copied to an array. 
/// </summary>
public class BinarySearcher<T> where T : IComparable<T>
{
    internal readonly T[] SortedElements;
    public T Min => SortedElements[0];
    public T Max => SortedElements[^1];
    public int Length => SortedElements.Length;

    public BinarySearcher(IEnumerable<T> data, bool alreadySorted = false)
    {
        SortedElements = alreadySorted ? data.ToArray() : data.OrderBy(x => x).ToArray();
    }

    public IEnumerable<T> QueryBetween(T lower, T upper, Func<T, bool>? predicate = null)
    {
        if (lower.CompareTo(upper) > 0)
            throw new ArgumentException("lower must be less than or equal to upper.");

        var startIndex = Array.BinarySearch(SortedElements, lower);
        if (startIndex < 0) startIndex = ~startIndex;

        var endIndex = Array.BinarySearch(SortedElements, startIndex, SortedElements.Length - startIndex, upper);
        if (endIndex < 0) endIndex = ~endIndex - 1;

        if (lower.CompareTo(upper) > 0)
            throw new Exception("RangeFinder: lower must be less than or equal to upper.");
        
        for (var i = startIndex; i <= endIndex; i++)
            if (predicate == null || predicate(SortedElements[i]))
                yield return SortedElements[i];
    }
}