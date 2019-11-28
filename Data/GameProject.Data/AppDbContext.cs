using System;
using System.Collections.Generic;
using System.Text;
using GnsGameProject.Data.Models.Game;
using GnsGameProject.Data.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GnsGameProject.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options)
        {
        }

        public DbSet<Word> Words { get; set; }

        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Match>()
                .HasOne(x => x.Word)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.WordId);

            builder.Entity<Match>()
                .HasOne(x => x.User)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.UserId);
        }
    }
}
