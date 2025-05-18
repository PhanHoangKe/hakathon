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
        public int? DocumentID { get; set; }
        public int? UserID { get; set; }
        public int? Rating { get; set; }
        public DateTime? RatingDate { get; set; }
        public string? Comment { get; set; }
        [ForeignKey("DocumentID")]
        public virtual tblDocuments? document { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUsers? user   { get; set; }


    }
}