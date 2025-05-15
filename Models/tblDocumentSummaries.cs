using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDocumentSummaries")]
    public class tblDocumentSummaries
    {
        [Key]
        public int SummaryID { get; set; }
        public int DocumentID { get; set; }
        public string SummaryText { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public string? GeneratedByModel { get; set; }
    }
}