using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblWebsiteLinks")]
    public class tblWebsiteLinks
    {
        [Key]
        public int LinkID { get; set; }
        public string LinkName { get; set; }
        public string URL { get; set; }
        public string? Description { get; set; }
        public int? ParentLinkID { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int? RequiredRoleID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}