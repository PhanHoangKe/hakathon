using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace hakathon.Models
{
    [Table("tblDownloadHistory")]
    public class tblDownloadHistory
    {
        [Key]
        public int DownloadID { get; set; }

		[ForeignKey("User")]
		public int UserID { get; set; }

		[ForeignKey("Documents")]
		public int DocumentID { get; set; }
        public DateTime DownloadDate { get; set; } = DateTime.Now;

		public virtual tblUsers User { get; set; }
		public virtual tblDocuments Document { get; set; }
	}
}