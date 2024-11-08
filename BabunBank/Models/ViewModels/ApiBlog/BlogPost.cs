﻿namespace BabunBank.Models.ViewModels.ApiBlog;

public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? LastModified { get; set; }

    public bool IsDeleted { get; set; }
}
