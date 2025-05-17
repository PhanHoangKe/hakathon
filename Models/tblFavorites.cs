using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblFavorites")]
    public class tblFavorites
    {
        [Key]
        public int FavoriteID { get; set; }
        public int UserID { get; set; }
        public int DocumentID { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public int? MenuID { get; set; }
        public tblUsers User { get; set; }
        public tblDocuments Document { get; set; }
    }
}