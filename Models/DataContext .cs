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
        public DbSet<tblCategories> tblCategories { get; set; }
        public DbSet<tblPublishers> tblPublishers { get; set; }
        public DbSet<tblAuthors> tblAuthors { get; set; }
        public DbSet<tblDocuments> tblDocuments { get; set; }
        public DbSet<tblViewHistory> tblViewHistories { get; set; }
        public DbSet<tblDocumentAuthors> tblDocumentAuthors { get; set; }
        public DbSet<tblFavorites> tblFavorites { get; set; }
        public DbSet<tblComments> tblComments { get; set; }
        public DbSet<tblDownloadHistory> tblDownloadHistories { get; set; }
        public DbSet<tblTransactionHistory> tblTransactionHistories { get; set; }
        public DbSet<tblFailedLoginAttempts> tblFailedLoginAttempts { get; set; }
        public DbSet<tblWebsiteLinks> tblWebsiteLinks { get; set; }
        public DbSet<tblCarouselImages> tblCarouselImages { get; set; }
        public DbSet<tblSystemConfig> tblSystemConfigs { get; set; }
        public DbSet<tblUserProfiles> tblUserProfiles { get; set; }
        public DbSet<tblContactMessages> tblContactMessages { get; set; }
        public DbSet<tblDocumentSummaries> tblDocumentSummaries { get; set; }
        public DbSet<tblDocumentAudio> tblDocumentAudios { get; set; }
        public DbSet<tblDocumentRatings> tblDocumentRatings { get; set; }
        public DbSet<tblChatbotMessages> tblChatbotMessages { get; set; }
        public DbSet<tblChatbotConversations> tblChatbotConversations { get; set; }
        public DbSet<viewCategoryMenu> viewCategoryMenus { get; set; }
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblUsersRoles>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });
            modelBuilder.Entity<tblDocumentAuthors>()
                .HasKey(da => new { da.DocumentID, da.AuthorID });
            base.OnModelCreating(modelBuilder);
        }
    }
}