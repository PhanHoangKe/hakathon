using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDocumentFeedback")]
    public class tblDocumentFeedback
    {

        [Key]
        public int FeedbackID { get; set; }
        public int? DocumentID { get; set; }
        public int? UserID { get; set; }
        public string? FeedbackType { get; set; }
        public string? FeedbackContent { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public bool? IsResolved { get; set; }
        public int? ResolvedByUserID { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string? ResponseContent { get; set; }
        [ForeignKey("DocumentID")]
        public virtual tblDocuments? document { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUsers? user   { get; set; }

    }
}