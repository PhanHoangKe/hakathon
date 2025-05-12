using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hakathon.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace hakathon.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        
        }

        public DbSet<tblMenu> tblMenus { get; set; }
        public DbSet<tblAdminMenu> tblAdminMenus { get; set; }
        public DbSet<tblUsers> tblUsers { get; set; }
        public DbSet<tblRoles> tblRoles { get; set; }
        public DbSet<tblUsersRoles> tblUsersRoles { get; set; }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblUsersRoles>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });
        
            base.OnModelCreating(modelBuilder);
        }
    }
}