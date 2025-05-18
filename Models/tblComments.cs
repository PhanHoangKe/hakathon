using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblComments")]
    public class tblComments
    {
        [Key]
        public int CommentID { get; set; }
        public int DocumentID { get; set; }
        public int UserID { get; set; }
        public string? CommentText { get; set; }
        public DateTime? CommentDate { get; set; }
        public bool? IsActive { get; set; }
       [ForeignKey("UserID")]
        public virtual tblUsers? user { get; set; }

        [ForeignKey("DocumentID")]
        public virtual tblDocuments? document { get; set; }
    }
}