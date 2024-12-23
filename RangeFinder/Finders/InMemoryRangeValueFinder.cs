using System.Collections.Immutable;
using System.Numerics;
using RangeFinder.Core;

namespace RangeFinder.Finders;

/// <summary>
/// Provides efficient range searches for a Dictionary with NumericRange as the key.
/// It is more efficient in terms of speed and memory usage for users to implement INumericRange
/// and perform searches using an IRangeFinder implementation.
/// </summary>
public class InMemoryRangeValueFinder<TNumber, TValue> where TNumber : INumber<TNumber>
{
    // Note that RangeFinder allows duplicated NumericRanges, but RangeValueFinder doesn't.
    // it's only instantiated from IDictionary keys
    private readonly InMemoryRangeFinder<TNumber> _inMemoryRangeFinder;
    
    // Note that the Key type is limited to NumericRange as it's used for keys of Dictionary.
    private readonly IImmutableDictionary<NumericRange<TNumber>, TValue> _dic;

    private InMemoryRangeValueFinder(IImmutableDictionary<NumericRange<TNumber>, TValue> dic)
    {
        _inMemoryRangeFinder = new InMemoryRangeFinder<TNumber>(dic.Keys);
        _dic = dic;
    }

    public static InMemoryRangeValueFinder<TNumber, TValue> 
        CreateInstance(IDictionary<NumericRange<TNumber>, TValue> dic)
    {
        // If dic is immutable, use it directly for faster instance creation and lower RAM consumption.
        if (dic is IImmutableDictionary<NumericRange<TNumber>, TValue> immutableDictionary)
            return new InMemoryRangeValueFinder<TNumber, TValue>(immutableDictionary);
        else
            return new InMemoryRangeValueFinder<TNumber, TValue>(dic.ToImmutableDictionary());
    }
    
    public IEnumerable<KeyValuePair<NumericRange<TNumber>, TValue>> 
        FindOverlappingRanges(NumericRange<TNumber> queryINumericRange, bool exceptTouching = false)
    {
        return _inMemoryRangeFinder
            .FindOverlappingRanges(queryINumericRange, exceptTouching)
            .Select(range =>
            {
                if (range is not NumericRange<TNumber> numericRange) throw new NullReferenceException();
                return new KeyValuePair<NumericRange<TNumber>, TValue>(
                    numericRange,
                    _dic[numericRange]);
            });
    }

    public IEnumerable<KeyValuePair<NumericRange<TNumber>, TValue>> 
        FindRangesBetween(NumericRange<TNumber> lower, NumericRange<TNumber> upper)
    {
        return _inMemoryRangeFinder
            .FindRangesBetween(lower, upper)
            .Select(range =>
            {
                if (range is not NumericRange<TNumber> numericRange) throw new NullReferenceException();
                return new KeyValuePair<NumericRange<TNumber>, TValue>(numericRange, _dic[numericRange]);
            });
    }
}