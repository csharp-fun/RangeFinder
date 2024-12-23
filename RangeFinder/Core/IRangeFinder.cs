using System.Numerics;

namespace RangeFinder.Core;

public interface IRangeFinder<TNumber> where TNumber : INumber<TNumber>
{
    public int Length { get; }

    public IEnumerable<INumericRange<TNumber>> FindOverlappingRanges(
        INumericRange<TNumber> queryNumericRange, 
        bool exceptTouching = false);

    public IEnumerable<INumericRange<TNumber>> FindRangesBetween(
        INumericRange<TNumber> lower,
        INumericRange<TNumber> upper);
}