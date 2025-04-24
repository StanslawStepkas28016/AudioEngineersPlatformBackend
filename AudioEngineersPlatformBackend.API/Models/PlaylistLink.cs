using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class PlaylistLink
{
    public int IdPlaylistLink { get; set; }

    public string Link { get; set; } = null!;

    public int IdAdvert { get; set; }

    public int IdPlaylistType { get; set; }

    public virtual Advert IdAdvertNavigation { get; set; } = null!;

    public virtual PlaylistTypeDict IdPlaylistTypeNavigation { get; set; } = null!;
}
