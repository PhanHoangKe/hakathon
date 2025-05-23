using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblCarouselImages")]
    public class tblCarouselImages
    {
        [Key]
        public int ImageID { get; set; }
        public string? ImageTitle { get; set; }
        public string ImagePath { get; set; }
        public string? Description { get; set; }
        public string? LinkURL { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}