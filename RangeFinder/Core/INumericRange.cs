using System.Numerics;

namespace RangeFinder.Core;

public interface INumericRange<TNumber> 
    : IComparable<INumericRange<TNumber>>
    where TNumber : INumber<TNumber>
{
    public TNumber Start { get; }
    public TNumber End { get; }
    public TNumber Span => End - Start;
    
    int IComparable<INumericRange<TNumber>>.CompareTo(INumericRange<TNumber>? other)
    {
        if (other == null) return 1; // null is always smaller than this
        var startComparison = Start.CompareTo(other.Start);
        return startComparison != 0
            ? startComparison
            : End.CompareTo(other.End);
    }
    
    public bool OverlapsIncludeTouching(INumericRange<TNumber> queryNumericRange) =>
        queryNumericRange.Start <= End && Start <= queryNumericRange.End;

    public bool OverlapsExceptTouching(INumericRange<TNumber> queryNumericRange) =>
        queryNumericRange.Start < End && Start < queryNumericRange.End;
}