using System.Numerics;

namespace RangeFinder.Core;

public interface IRangeFinder<TNumber, out TCustomRange>
    where TNumber : INumber<TNumber>
    where TCustomRange: INumericRange<TNumber>
{
    public int Length { get; }

    public IEnumerable<TCustomRange> FindOverlappingRanges(
        INumericRange<TNumber> queryNumericRange, 
        bool exceptTouching = false);

    public IEnumerable<TCustomRange> FindRangesBetween(
        INumericRange<TNumber> lower,
        INumericRange<TNumber> upper);
}