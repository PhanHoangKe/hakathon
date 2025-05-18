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

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Documents")]
        public int DocumentID { get; set; }
        public DateTime ViewDate { get; set; } = DateTime.Now;
        public int? ViewDuration { get; set; }
        public int? LastPageViewed { get; set; }
        public virtual tblUsers User { get; set; }
        public virtual tblDocuments Document { get; set; }
    }
}