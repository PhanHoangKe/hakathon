using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblUserProfiles")]
    public class tblUserProfiles
    {
        [Key, ForeignKey("User")]
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public DateTime? LastLoginDate { get; set; }
        public int FailedLoginCount { get; set; } = 0;
        public DateTime? LockoutEndDate { get; set; }
    }
}