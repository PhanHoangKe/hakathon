using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblFailedLoginAttempts")]
    public class tblFailedLoginAttempts
    {
        [Key]
        public int AttemptID { get; set; }
        public int? UserID { get; set; }
        public string? Username { get; set; }
        public string? IPAddress { get; set; }
        public DateTime? AttemptDate { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUsers? user { get; set; }

    }
}