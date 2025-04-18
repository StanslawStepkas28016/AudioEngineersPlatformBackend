using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class SocialMediaLink
{
    public int IdSocialMediaLink { get; set; }

    public string SocialMediaLink1 { get; set; } = null!;

    public int IdUser { get; set; }

    public int IdSocialMediaTypeDict { get; set; }

    public virtual SocialMediaTypeDict IdSocialMediaTypeDictNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
