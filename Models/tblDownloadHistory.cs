using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDownloadHistory")]
    public class tblDownloadHistory
    {
        [Key]
        public int DownloadID { get; set; }
        public int UserID { get; set; }
        public int DocumentID { get; set; }
        public DateTime DownloadDate { get; set; } = DateTime.Now;
    }
}