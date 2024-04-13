using Babun_API.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Babun_API.Data;

public class DataInitializer(ApiContext dbContext)
{
    private const int SeedingVariable = 100;

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
        if (!await dbContext.Ads.AnyAsync())
        {
            for (var i = 0; i < SeedingVariable; i++)
            {
                try
                {
                    var ad = new Ad
                    {
                        Title = faker.Company.Bs(),
                        Author = faker.Name.FullName(),
                        Description = faker.Lorem.Sentence()[..15],
                        Content = faker.Lorem.Paragraphs(random.Next(5, 9)),
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

    private async Task CreateTableIfNotExists()
    {
        throw new NotImplementedException();
    }
}
