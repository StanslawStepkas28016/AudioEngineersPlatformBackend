using System;
using System.Collections.Generic;

namespace AudioEngineersPlatformBackend.Models;

public partial class UserLog
{
    public int IdUserLog { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateDeleted { get; set; }

    public bool IsDeleted { get; set; }

    public string? VerificationCode { get; set; }

    public DateTime? VerificationCodeExpiration { get; set; }

    public bool IsVerified { get; set; }

    public DateTime? DateLastLogin { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
