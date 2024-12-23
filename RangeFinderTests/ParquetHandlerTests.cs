using System.Collections.Concurrent;
using System.Diagnostics;
using RangeFinder;
using RangeFinder.Core;
using RangeFinder.Finders;
using RangeFinder.Utilities;

namespace RangeFinderTests;

public class ParquetHandlerTests
{
    private const string ParquetFile = "ParquetHandlerTests.parquet";

    [SetUp]
    public void Setup()
    {
        var ranges = new List<NumericRange<double>>
        {
            new (1.0, 2.2),
            new (2.0, 3.2),
            new (3.0, 3.2),
        };

        ParquetHandler.SaveToParquet(ranges,ParquetFile);
    }

    [Test]
    public void LoadParquetTest()
    {
        var ranges = ParquetHandler.LoadFromParquet<NumericRange<double>>(ParquetFile);
        Assert.That(ranges.Count(), Is.EqualTo(3));
    }
}