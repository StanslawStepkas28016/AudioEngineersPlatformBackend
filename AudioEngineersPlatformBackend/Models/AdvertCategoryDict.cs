using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class AdvertCategoryDict
{
    public int IdAdvertCategoryDict { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Advert> IdAdverts { get; set; } = new List<Advert>();
}
