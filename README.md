# RangeFinder

A Fairly Fast Range Query Library in C#

## Features

- Define custom range types using numeric types.
- Perform high-speed range overlap queries.

## Quick Example

```C#
// Define a custom type inheriting a numeric range with additional properties
record UserDefinedNumericRange(
    double Start,
    double End,
    string UserAddedExtraMember
    ) : INumericRange<double>;

// Create a RangeFinder instance
var ranges = new List<UserDefinedNumericRange>
{
    new(1.0, 2.2, "Extra: [1.0,2.2]"),
    new(2.0, 3.2, "Extra: [2.0,3.2]"),
    new(3.0, 3.2, "Extra: [3.0,3.2]"),
};
var rangeFinder = new InMemoryRangeFinder<double, UserDefinedNumericRange>(ranges);

// Example query
var resultsExceptTouching = rangeFinder.FindOverlappingRanges(
    new NumericRange<double>(2.0, 3.0), 
    exceptTouching: true);

foreach (UserDefinedNumericRange range in resultsExceptTouching)
    Console.WriteLine(range.UserAddedExtraMember); 
// Output:
// Extra: [1.0, 2.2]
// Extra: [2.0, 3.2]
```

## Preliminary Benchmark Results

Performance measured on M1 Mac mini with 16GB RAM It was approximately 10 times faster than
in-memory searches on a sorted dataset using DuckDB, but further verification of the measurement accuracy is pending.

| Dataset Size | Threads | Queries | Execution Time (ms) | Avg Results size |
|--------------|---------|---------|---------------------|------------------|
| 10M elements | Multi   | 100,000 | 20,905              | 12,070           |
| 5M elements  | Multi   | 100,000 | 4,733               | 6,047            |
| 5M elements  | Single  | 10,000  | 3,558               | 5,897            |

Have fun!
