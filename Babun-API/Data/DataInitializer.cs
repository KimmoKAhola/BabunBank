using Babun_API.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Data;

/// <summary>
/// Initializes and seeds the database with initial data.
/// </summary>
public class DataInitializer(ApiContext dbContext)
{
    private const int NumberOfAdsToSeed = 200;

    /// <summary>
    /// Seeds initial data into the database if it does not already exist.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SeedData()
    {
        try
        {
            await dbContext.Database.MigrateAsync();
            // await CreateTableIfNotExists();
            await CreateDataIfNotExists();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task CreateDataIfNotExists()
    {
        var faker = new Faker();
        var random = new Random();
        if (await dbContext.Ads.CountAsync() < NumberOfAdsToSeed)
        {
            for (var i = 0; i < NumberOfAdsToSeed; i++)
            {
                try
                {
                    var ad = new Ads
                    {
                        Title = faker.Company.Bs(),
                        Author = faker.Name.FullName(),
                        Description = faker.Lorem.Sentence()[..30],
                        Content = faker.Lorem.Paragraphs(random.Next(5, 30)),
                        DateCreated = faker.Date.Past()
                    };
                    await dbContext.Ads.AddAsync(ad);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }

    //TODO check if this is needed
    private async Task CreateTableIfNotExists()
    {
        throw new NotImplementedException();
    }
}
