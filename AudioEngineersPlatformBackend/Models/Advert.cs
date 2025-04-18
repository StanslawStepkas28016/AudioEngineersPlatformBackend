using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class Advert
{
    public int IdAdvert { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public int IdUser { get; set; }

    public int IdAdvertLog { get; set; }

    public virtual AdvertLog IdAdvertLogNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<PlaylistLink> PlaylistLinks { get; set; } = new List<PlaylistLink>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<AdvertCategoryDict> IdAdvertCategoryDicts { get; set; } = new List<AdvertCategoryDict>();
}
