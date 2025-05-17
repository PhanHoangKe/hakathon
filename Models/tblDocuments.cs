using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDocuments")]
    public class tblDocuments
    {
        [Key]
        public int DocumentID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CategoryID { get; set; }
        public int? PublisherID { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string FileOriginalPath { get; set; }
        public string FilePDFPath { get; set; }
        public long? FileSize { get; set; }
        public string? FileType { get; set; }
        public int ViewCount { get; set; } = 0;
        public int DownloadCount { get; set; } = 0;
        public bool IsApproved { get; set; } = false;
        public int UserID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int? MenuID { get; set; }
        public tblCategories Category { get; set; }
        public tblPublishers Publisher { get; set; }
        public ICollection<tblDocumentAuthors> DocumentAuthor { get; set; }
        public ICollection<tblFavorites> Favorite { get; set; }
    }
}