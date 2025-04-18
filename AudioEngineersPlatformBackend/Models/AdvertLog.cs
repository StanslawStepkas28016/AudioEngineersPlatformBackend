using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class AdvertLog
{
    public int IdAdvertLog { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateDeleted { get; set; }

    public DateTime? DateModified { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int IdUserModifier { get; set; }

    public virtual ICollection<Advert> Adverts { get; set; } = new List<Advert>();

    public virtual User IdUserModifierNavigation { get; set; } = null!;
}
