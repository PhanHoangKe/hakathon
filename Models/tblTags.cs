using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblTags")]
    public class tblTags
    {
        [Key]
        public int TagID { get; set; }
        public string? TagName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }

    }
}