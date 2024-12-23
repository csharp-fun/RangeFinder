using System.Numerics;
using RangeFinder.Core;

namespace RangeFinder.Finders;

public class ParquetRangeFinder<TNumber> : IRangeFinder<TNumber>
    where TNumber : INumber<TNumber>
{
    public int Length => throw new NotImplementedException();
    
    public ParquetRangeFinder(IEnumerable<INumericRange<TNumber>> ranges, string parquetFilePath)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<INumericRange<TNumber>> FindOverlappingRanges(
        INumericRange<TNumber> queryRange, bool exceptTouching = false)
    { 
        throw new NotImplementedException();
    }

    public IEnumerable<INumericRange<TNumber>> FindRangesBetween(
        INumericRange<TNumber> lower, 
        INumericRange<TNumber> upper)
    { 
        throw new NotImplementedException();
    }
}