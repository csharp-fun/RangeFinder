using Parquet.Serialization;
using RangeFinder.Core;

namespace RangeFinder.Utilities;

public static class ParquetHandler
{
    public static void SaveToParquet<T>(IEnumerable<T> data, string fileName)
    {
        ParquetSerializer.SerializeAsync(data, fileName).Wait();
    }

    public static IEnumerable<T> LoadFromParquet<T>(string fileName) where T : new()
    {
        return ParquetSerializer.DeserializeAsync<T>(fileName).Result;
    }
}
