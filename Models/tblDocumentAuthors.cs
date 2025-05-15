using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblDocumentAuthors")]
    public class tblDocumentAuthors
    {
        public int DocumentID { get; set; }
        public int AuthorID { get; set; }
        public tblDocuments Document { get; set; }
        public tblAuthors Author { get; set; }
    }
}