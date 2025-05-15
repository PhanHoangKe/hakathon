using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblSystemConfig")]
    public class tblSystemConfig
    {
        [Key]
        public int ConfigID { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string? Description { get; set; }
        public string DataType { get; set; } = "string";
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int? ModifiedByUserID { get; set; }
    }
}