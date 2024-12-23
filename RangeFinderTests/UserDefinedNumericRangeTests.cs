using System.Collections.Concurrent;
using System.Diagnostics;
using RangeFinder;
using RangeFinder.Core;
using RangeFinder.Finders;

namespace RangeFinderTests;

public class UserDefinedNumericRangeTests
{
    private record UserDefinedNumericRange
        (double Start, double End, string UserAddedExtraMember) : INumericRange<double>;
    
    private InMemoryRangeFinder<double, UserDefinedNumericRange> _rangeFinder = null!;

    [SetUp]
    public void Setup()
    {
        var ranges = new List<UserDefinedNumericRange>
        {
            new(1.0, 2.2, "Extra: [1.0,2.2]"),
            new(2.0, 3.2, "Extra: [2.0,3.2]"),
            new(3.0, 3.2, "Extra: [3.0,3.2]"),
        };
        _rangeValueFinder = new InMemoryRangeFinder<double>(ranges);
        _rangeFinder = new InMemoryRangeFinder<double, UserDefinedNumericRange>(ranges);

    }

    [Test]
    public void FindOverlappingElementsTest()
    {
        var overlapped = _rangeFinder
            .FindOverlappingRanges(new NumericRange<double>(2.5, 5.1)).ToArray();
        Assert.That(overlapped, Has.Length.EqualTo(2));
        
        overlapped = _rangeFinder
            .FindOverlappingRanges(new NumericRange<double>(2.5, 5.1), exceptTouching:true)
            .ToArray();
        Assert.That(overlapped, Has.Length.EqualTo(2));
    }
}