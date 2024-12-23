using System.Collections.Concurrent;
using System.Diagnostics;
using RangeFinder;
using RangeFinder.Core;
using RangeFinder.Finders;

namespace RangeFinderTests;

public class InMemoryRangeFinderTests
{
    private IRangeFinder<double> _inMemoryRangeFinder = null!;

    [SetUp]
    public void Setup()
    {
        var ranges = new List<NumericRange<double>>
        {
            new(1.0, 2.2), new(2.0, 2.5),
            new(1.0, 4.0), new(4.0, 5.0),
            new(5.0, 6.0), new(6.0, 20.0)
        };
        _inMemoryRangeFinder = new InMemoryRangeFinder<double>(ranges);
    }

    [Test]
    public void FindOverlappingElementsTest()
    {
        var overlapped = _inMemoryRangeFinder
            .FindOverlappingRanges(new NumericRange<double>(2.5, 5.1)).ToArray();
        Assert.That(overlapped, Has.Length.EqualTo(4));
        
        overlapped = _inMemoryRangeFinder
            .FindOverlappingRanges(new NumericRange<double>(2.5, 5.1), exceptTouching:true).ToArray();
        Assert.That(overlapped, Has.Length.EqualTo(3));
    }
    
    [Test]
    public void FindOverlappingElementsTest2()
    {
        var overlapped = _inMemoryRangeFinder
            .FindOverlappingRanges(new NumericRange<double>(-5, 4.0)).ToArray();
        Assert.That(overlapped, Has.Length.EqualTo(3));
    }

    [Test]
    public void FindOverlappedElements_SpeedTest()
    {
        var largeOverlapFinder = new InMemoryRangeFinder<double>(GenerateRandomRanges(5_000_000, 0, 5000));
        const int totalQueryNumber = 100_000;
        var resultsCounter = new ConcurrentBag<int>();
        var sw = new Stopwatch();
        sw.Start();
        Parallel.For(0, totalQueryNumber, i =>
        {
            var queryRange = new NumericRange<double>(2000 + i * 0.0001, 2000.0001 + i * 0.0001);
            var overlapped = largeOverlapFinder.FindOverlappingRanges(queryRange).ToArray();
            resultsCounter.Add(overlapped.Length);
        });
        sw.Stop();
        Console.WriteLine($"{totalQueryNumber:N0} queries issued in {sw.Elapsed.TotalMilliseconds:N}[ms]");
        Console.WriteLine($"Average, Max, Min number of results:" +
                          $"{resultsCounter.Average()}, {resultsCounter.Max()}, {resultsCounter.Min()}");
    }

    private static List<NumericRange<double>> GenerateRandomRanges(int n, double min, double max)
    {
        if (min >= max)
            throw new ArgumentException("min must be less than max.");

        var random = new Random();
        var ranges = new List<NumericRange<double>>();

        for (var i = 0; i < n; i++)
        {
            var start = random.NextDouble() * (max - min) + min;
            var end = random.NextDouble() * (max - min) + min;

            // Ensure start is less than or equal to end
            if (start > end)
                (start, end) = (end, start);

            end = start + end % 10;

            ranges.Add(new NumericRange<double>(start, end));
        }
        return ranges;
    }

}