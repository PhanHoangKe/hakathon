using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    public class tblFailedLoginAttempts
    {
        [Key]
        public int AttemptID { get; set; }
        public int? UserID { get; set; }
        public string Username { get; set; }
        public string? IPAddress { get; set; }
        public DateTime AttemptDate { get; set; } = DateTime.Now;
    }
}