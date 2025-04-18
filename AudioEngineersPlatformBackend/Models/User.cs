using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class User
{
    public int IdUser { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdRole { get; set; }

    public int IdUserLog { get; set; }

    public virtual ICollection<AdvertLog> AdvertLogs { get; set; } = new List<AdvertLog>();

    public virtual ICollection<Advert> Adverts { get; set; } = new List<Advert>();

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual UserLog IdUserLogNavigation { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<SocialMediaLink> SocialMediaLinks { get; set; } = new List<SocialMediaLink>();
}
