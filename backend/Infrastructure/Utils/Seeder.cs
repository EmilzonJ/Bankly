using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Utils;

public class Seeder<T>(IMongoCollection<T> collection)
{
    public async Task SeedAsync(string filePath)
    {
        var records = GetRecordsAsync(filePath);

        if (records.Count > 0)
        {
            await collection.InsertManyAsync(records);
        }
    }

    public async Task SeedAsync(List<T> records)
    {
        if (records.Count > 0)
        {
            await collection.InsertManyAsync(records);
        }
    }

    public List<T> GetRecordsAsync(string filePath)
    {
        using var reader = new StreamReader(filePath);
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            HeaderValidated = null,
            MissingFieldFound = null,
            IgnoreReferences = true,
            Delimiter = ","
        };

        using var csvReader = new CsvReader(reader, csvConfig);
        var records = csvReader.GetRecords<T>().ToList();

        return records;
    }
}
