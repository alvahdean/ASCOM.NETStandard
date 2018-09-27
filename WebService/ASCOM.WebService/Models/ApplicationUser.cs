using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ASCOM.WebService.Models
{
    //public class ApplicationUser : IdentityUser, IRaciUser
    //{
    //    public static implicit operator RaciUser(ApplicationUser user)
    //    {
    //        return new RaciUser()
    //        {
    //            UserSettingsId = user.UserSettingsId,
    //            UserSettings = user.UserSettings,
    //            LockoutEnd = user.LockoutEnd,
    //            TwoFactorEnabled = user.TwoFactorEnabled,
    //            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
    //            PhoneNumber = user.PhoneNumber,
    //            ConcurrencyStamp = user.ConcurrencyStamp,
    //            SecurityStamp = user.SecurityStamp,
    //            PasswordHash = user.PasswordHash,
    //            EmailConfirmed = user.EmailConfirmed,
    //            NormalizedEmail = user.NormalizedEmail,
    //            Email = user.Email,
    //            NormalizedUserName = user.NormalizedUserName,
    //            UserName = user.UserName,
    //            Id = user.Id,
    //            LockoutEnabled = user.LockoutEnabled,
    //            AccessFailedCount = user.AccessFailedCount
    //        };
    //    }
    //    public static implicit operator ApplicationUser(RaciUser user)
    //    {
    //        return new ApplicationUser()
    //        {
    //            UserSettingsId = user.UserSettingsId,
    //            UserSettings = user.UserSettings,
    //            LockoutEnd = user.LockoutEnd,
    //            TwoFactorEnabled = user.TwoFactorEnabled,
    //            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
    //            PhoneNumber = user.PhoneNumber,
    //            ConcurrencyStamp = user.ConcurrencyStamp,
    //            SecurityStamp = user.SecurityStamp,
    //            PasswordHash = user.PasswordHash,
    //            EmailConfirmed = user.EmailConfirmed,
    //            NormalizedEmail = user.NormalizedEmail,
    //            Email = user.Email,
    //            NormalizedUserName = user.NormalizedUserName,
    //            UserName = user.UserName,
    //            Id = user.Id,
    //            LockoutEnabled = user.LockoutEnabled,
    //            AccessFailedCount = user.AccessFailedCount
    //        };
    //    }

    //    public int UserSettingsId { get; set; }
    //    public UserSettings UserSettings { get; set; }

    //}

    public class RaciUser : IdentityUser, IRaciUser
    {
        public static implicit operator Raci.Data.RaciUser(ASCOM.WebService.Models.RaciUser user)
        {
            return new RaciUser()
            {
                UserSettingsId = user.UserSettingsId,
                UserSettings = user.UserSettings,
                LockoutEnd = user.LockoutEnd,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                ConcurrencyStamp = user.ConcurrencyStamp,
                SecurityStamp = user.SecurityStamp,
                PasswordHash = user.PasswordHash,
                EmailConfirmed = user.EmailConfirmed,
                NormalizedEmail = user.NormalizedEmail,
                Email = user.Email,
                NormalizedUserName = user.NormalizedUserName,
                UserName = user.UserName,
                Id = user.Id,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount
            };
        }
        public static implicit operator ASCOM.WebService.Models.RaciUser(Raci.Data.RaciUser user)
        {
            return new ApplicationUser()
            {
                UserSettingsId = user.UserSettingsId,
                UserSettings = user.UserSettings,
                LockoutEnd = user.LockoutEnd,
                TwoFactorEnabled = user.TwoFactorEnabled,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber,
                ConcurrencyStamp = user.ConcurrencyStamp,
                SecurityStamp = user.SecurityStamp,
                PasswordHash = user.PasswordHash,
                EmailConfirmed = user.EmailConfirmed,
                NormalizedEmail = user.NormalizedEmail,
                Email = user.Email,
                NormalizedUserName = user.NormalizedUserName,
                UserName = user.UserName,
                Id = user.Id,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount
            };
        }

        public int UserSettingsId { get; set; }
        public UserSettings UserSettings { get; set; }

    }
}
