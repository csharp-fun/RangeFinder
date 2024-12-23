using System.Collections.Concurrent;
using System.Diagnostics;
using RangeFinder;
using RangeFinder.Core;
using RangeFinder.Finders;

namespace RangeFinderTests;

public class InMemoryRangeValueFinderTests
{
    private InMemoryRangeValueFinder<double, string> _inMemoryRangeValueFinder = null!;

    [SetUp]
    public void Setup()
    {
        var ranges = new Dictionary<NumericRange<double>, string>
        {
            {new NumericRange<double>(1.0, 2.2),"[1.0,2.2]"},
            {new NumericRange<double>(2.0, 2.5),"[2.0,2.5]"},
            {new NumericRange<double>(1.0, 4.0),"[1.0,4.0]"},
        };
        _inMemoryRangeValueFinder = InMemoryRangeValueFinder<double,string>.CreateInstance(ranges);
    }

    [Test]
    public void FindOverlappingElementsTest()
    {
        var overlapped = _inMemoryRangeValueFinder
            .FindOverlappingRanges(new NumericRange<double>(2.5, 5.1)).ToArray();
        Assert.That(overlapped, Has.Length.EqualTo(2));
        
        overlapped = _inMemoryRangeValueFinder
            .FindOverlappingRanges(new NumericRange<double>(2.5, 5.1), exceptTouching:true).ToArray();
        Assert.That(overlapped, Has.Length.EqualTo(1));
    }
}