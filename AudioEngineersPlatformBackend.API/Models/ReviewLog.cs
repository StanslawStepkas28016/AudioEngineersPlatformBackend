using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class ReviewLog
{
    public int IdReviewLog { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateDeleted { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
