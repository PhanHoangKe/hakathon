using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDocumentTags")]
    public class tblDocumentTags
    {
        [Key]
        public int DocumentID { get; set; }
        public int TagID { get; set; }
       
        [ForeignKey("DocumentID")]
        public virtual tblDocuments? document { get; set; }
        [ForeignKey("TagID")]
        public virtual tblTags? tag  { get; set; }


    }
}