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
        public DbSet<tblCategories> Categories { get; set; }
        public DbSet<tblPublishers> Publishers { get; set; }
        public DbSet<tblAuthors> tblAuthors { get; set; }
        public DbSet<tblDocuments> tblDocuments { get; set; }
        public DbSet<tblCarouselImages> carouselImages { get; set; }
        public DbSet<tblComments> Comments { get; set; }
        public DbSet<tblDocumentAudio> DocumentAudios { get; set; }
        public DbSet<tblDocumentAuthors> tblDocumentAuthors { get; set; }
        public DbSet<tblDocumentSummaries> DocumentSummaries { get; set; }
        public DbSet<tblDownloadHistory> DownloadHistories { get; set; }
        public DbSet<tblFailedLoginAttempts> FailedLoginAttempts { get; set; }
        public DbSet<tblFavorites> tblFavorites { get; set; }
        public DbSet<tblSystemConfig> SystemConfigs { get; set; }
        public DbSet<tblTransactionHistory> TransactionHistories { get; set; }
        public DbSet<tblUserProfiles> UserProfiles { get; set; }
        public DbSet<tblChatbotConversations> chatbotConversations { get; set; }
        public DbSet<tblChatbotMessages> ChatbotMessages { get; set; }
        public DbSet<tblWebsiteLinks> websiteLinks { get; set; }
        public DbSet<tblContactMessages> ContactMessages { get; set; }
        public DbSet<tblViewHistory> viewHistories { get; set; }
        public DbSet<tblTags> tags { get; set; }
        public DbSet<tblDocumentFeedback> tblDocumentFeedbacks { get; set; }
        public DbSet<tblDocumentRatings> tblDocumentRatings { get; set; }
        public DbSet<tblDocumentTags> tblDocumentTag { get; set; }
        public DbSet<tblUserNotifications> tblUserNotifications { get; set; }
        public DbSet<viewFavoriteDocumentMenu> viewFavoriteDocumentMenus { get; set; }
        public DbSet<tblUsers> tblUser { get; set; }
        public DbSet<viewCategoryMenu> viewCategoryMenus { get; set; }
        public DbSet<viewDocumentAuthor>viewDocumentAuthors { get; set; }
           public DbSet<Emails>Emails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblUsersRoles>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            base.OnModelCreating(modelBuilder);
        }
    }
}