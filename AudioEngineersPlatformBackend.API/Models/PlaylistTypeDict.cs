using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class PlaylistTypeDict
{
    public int IdPlaylistTypeDict { get; set; }

    public string PlaylistTypeName { get; set; } = null!;

    public virtual ICollection<PlaylistLink> PlaylistLinks { get; set; } = new List<PlaylistLink>();
}
