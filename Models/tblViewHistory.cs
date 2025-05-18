using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblViewHistory")]
    public class tblViewHistory
    {
        [Key]
        public int ViewID { get; set; }
        public int? UserID { get; set; }
        public int? DocumentID { get; set; }
        public DateTime? ViewDate { get; set; }
        public int? ViewDuration { get; set; }
        public int? LastPageViewed { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUsers? user { get; set; }
        [ForeignKey("DocumentID")]
        public virtual tblDocuments? document { get; set; }

    }
}