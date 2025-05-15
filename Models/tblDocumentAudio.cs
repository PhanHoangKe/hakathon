using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDocumentAudio")]
    public class tblDocumentAudio
    {
        [Key]
        public int AudioID { get; set; }
        public int DocumentID { get; set; }
        public string AudioPath { get; set; }
        public int? Duration { get; set; }
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
        public string? GeneratedByModel { get; set; }
    }
}