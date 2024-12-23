using System.Numerics;
using RangeFinder.Core;
using RangeFinder.Utilities;

namespace RangeFinder.Finders;

/// <summary>
/// Provides an in-memory implementation of <see cref="IRangeFinder{TNumber}"/> that efficiently finds
/// overlapping ranges and ranges between specified boundaries in a collection of <see cref="INumericRange{TNumber}"/>.
/// </summary>
/// <typeparam name="TNumber">The type of number used in the ranges.</typeparam>
/// <typeparam name="TCustomRange">Custom numeric range implemented by users.</typeparam>
public class InMemoryRangeFinder<TNumber, TCustomRange> : IRangeFinder<TNumber, TCustomRange>
    where TNumber : INumber<TNumber> 
    where TCustomRange: INumericRange<TNumber>
{
    public int Length => _binarySearcher.Length;
    
    private readonly BinarySearcher<INumericRange<TNumber>> _binarySearcher;
    private readonly TNumber _maxSpanOfTheRangesForPruning;

    public InMemoryRangeFinder(IEnumerable<TCustomRange> ranges)
    {
        _binarySearcher = new BinarySearcher<INumericRange<TNumber>>((IEnumerable<INumericRange<TNumber>>)ranges);
        _maxSpanOfTheRangesForPruning = _binarySearcher
            .SortedElements
            .MaxBy(range => range.Span)!
            .Span ?? throw new InvalidOperationException("Could not max find range span for pruning");
    }

    public IEnumerable<TCustomRange> FindOverlappingRanges(
        INumericRange<TNumber> queryRange, bool exceptTouching = false)
    {
        // Start searching from the above to find overlapping ranges effectively
        var prunedRangeStart = queryRange.Start - _maxSpanOfTheRangesForPruning;
        if (prunedRangeStart < TNumber.Zero) prunedRangeStart = TNumber.Zero;

        Func<INumericRange<TNumber>,bool> predicateToUse = exceptTouching 
            ? queryRange.OverlapsExceptTouching 
            : queryRange.OverlapsIncludeTouching;
        
        return (_binarySearcher
            .QueryBetween(
                lower: new NumericRange<TNumber>(prunedRangeStart, queryRange.Start),
                upper: new NumericRange<TNumber>(queryRange.End, queryRange.End),
                predicate: predicateToUse) as IEnumerable<TCustomRange>)!;
    }

    public IEnumerable<TCustomRange> FindRangesBetween(
        INumericRange<TNumber> lower, 
        INumericRange<TNumber> upper)
    {
        return (_binarySearcher.QueryBetween(lower, upper) as IEnumerable<TCustomRange>)!;
    }
}