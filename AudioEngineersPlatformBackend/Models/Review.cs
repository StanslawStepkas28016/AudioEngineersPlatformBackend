using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class Review
{
    public int IdReview { get; set; }

    public string Content { get; set; } = null!;

    public byte SatisfactionLevel { get; set; }

    public int IdUserReviewer { get; set; }

    public int IdAdvert { get; set; }

    public int IdReviewLog { get; set; }

    public virtual Advert IdAdvertNavigation { get; set; } = null!;

    public virtual ReviewLog IdReviewLogNavigation { get; set; } = null!;

    public virtual User IdUserReviewerNavigation { get; set; } = null!;
}
