using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class FileBankDbContext : IdentityDbContext<AppUser>
    {
        public FileBankDbContext(DbContextOptions<FileBankDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }

        public DbSet<FileType> FileTypes { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            /*builder.Entity<FileUpload>()
                .HasOne(f => f.FileType)
                .WithMany(f => f.FileUploads)
                .HasForeignKey(f => f.FileTypeId)
                .OnDelete(DeleteBehavior.Cascade);*/
        }

    }
}
