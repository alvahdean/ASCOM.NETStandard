using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Data
{
    public interface IIdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        //
        // Summary:
        //     Gets or sets the date and time, in UTC, when any user lockout ends.
        //
        // Remarks:
        //     A value in the past means the user is not locked out.
        DateTimeOffset? LockoutEnd { get; set; }
        //
        // Summary:
        //     Gets or sets a flag indicating if two factor authentication is enabled for this
        //     user.
        bool TwoFactorEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets a flag indicating if a user has confirmed their telephone address.
        bool PhoneNumberConfirmed { get; set; }
        //
        // Summary:
        //     Gets or sets a telephone number for the user.
        string PhoneNumber { get; set; }
        //
        // Summary:
        //     A random value that must change whenever a user is persisted to the store
        string ConcurrencyStamp { get; set; }
        //
        // Summary:
        //     A random value that must change whenever a users credentials change (password
        //     changed, login removed)
        string SecurityStamp { get; set; }
        //
        // Summary:
        //     Gets or sets a salted and hashed representation of the password for this user.
        string PasswordHash { get; set; }
        //
        // Summary:
        //     Gets or sets a flag indicating if a user has confirmed their email address.
        bool EmailConfirmed { get; set; }
        //
        // Summary:
        //     Gets or sets the normalized email address for this user.
        string NormalizedEmail { get; set; }
        //
        // Summary:
        //     Gets or sets the email address for this user.
        string Email { get; set; }
        //
        // Summary:
        //     Gets or sets the normalized user name for this user.
        string NormalizedUserName { get; set; }
        //
        // Summary:
        //     Gets or sets the user name for this user.
        string UserName { get; set; }
        //
        // Summary:
        //     Gets or sets the primary key for this user.
        TKey Id { get; set; }
        //
        // Summary:
        //     Gets or sets a flag indicating if the user could be locked out.
        bool LockoutEnabled { get; set; }
        //
        // Summary:
        //     Gets or sets the number of failed login attempts for the current user.
        int AccessFailedCount { get; set; }
    }
}
