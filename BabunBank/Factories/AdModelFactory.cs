using BabunBank.Models.FormModels.AdModels;
using BabunBank.Models.ViewModels.ApiBlog;

namespace BabunBank.Factories;

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
