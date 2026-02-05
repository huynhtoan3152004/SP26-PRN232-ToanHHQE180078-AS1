using System;
using System.Collections.Generic;

namespace HuynhHuuToan__SE1856_A01_Repository.Models.Entities;

public partial class NewsArticle
{
    public string NewsArticleID { get; set; } = null!;

    public string NewsTitle { get; set; } = null!;

    public string? Headline { get; set; }

    public DateTime CreatedDate { get; set; }

    public string NewsContent { get; set; } = null!;

    public string? NewsSource { get; set; }

    public int CategoryID { get; set; }

    public bool NewsStatus { get; set; }

    public int CreatedByID { get; set; }

    public int? UpdatedByID { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual SystemAccount CreatedBy { get; set; } = null!;

    public virtual SystemAccount? UpdatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
