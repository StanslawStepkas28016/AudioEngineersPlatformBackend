using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class SocialMediaTypeDict
{
    public int IdSocialMediaType { get; set; }

    public string SocialMediaTypeName { get; set; } = null!;

    public virtual ICollection<SocialMediaLink> SocialMediaLinks { get; set; } = new List<SocialMediaLink>();
}
