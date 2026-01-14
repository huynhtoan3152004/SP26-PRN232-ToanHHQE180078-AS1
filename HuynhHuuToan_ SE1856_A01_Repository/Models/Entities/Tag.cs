using System;
using System.Collections.Generic;

namespace HuynhHuuToan__SE1856_A01_Repository.Models.Entities;

public partial class Tag
{
    public int TagID { get; set; }

    public string TagName { get; set; } = null!;

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
