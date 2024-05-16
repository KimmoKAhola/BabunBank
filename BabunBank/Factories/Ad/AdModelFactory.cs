using BabunBank.Models.FormModels.Ad;
using BabunBank.Models.ViewModels.ApiBlog;

namespace BabunBank.Factories.Ad;

public static class AdModelFactory
{
    public static EditAdModel Create(BlogPost blogpost)
    {
        return new EditAdModel
        {
            Title = blogpost.Title,
            Author = blogpost.Author,
            Content = blogpost.Content,
            Description = blogpost.Description,
            IsDeleted = false
        };
    }
}
