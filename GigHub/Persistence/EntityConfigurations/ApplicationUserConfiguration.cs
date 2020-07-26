using GigHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace GigHub.Persistence.EntityConfigurations
{
    public class ApplicationUserConfiguration:EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
            Property(au => au.Name)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(au => au.Followers)
                .WithRequired(f => f.Followee)
                .WillCascadeOnDelete(false);

            HasMany(au => au.Followees)
                .WithRequired(f => f.Follower)
                .WillCascadeOnDelete(false);

            HasMany(au => au.UserNotifications)
                .WithRequired(un => un.User)
                .WillCascadeOnDelete(false);
        }
    }
}