using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class Image
{
    public int IdImage { get; set; }

    public string ImageLink { get; set; } = null!;

    public int IdAdvert { get; set; }

    public virtual Advert IdAdvertNavigation { get; set; } = null!;
}
