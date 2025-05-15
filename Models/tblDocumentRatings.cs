using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDocumentRatings")]
    public class tblDocumentRatings
    {
        [Key]
        public int RatingID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public DateTime RatingDate { get; set; } = DateTime.Now;
        public string? Comment { get; set; }
        public tblDocuments? Document { get; set; }
        public tblUsers? User { get; set; }
    }
}