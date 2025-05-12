using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblUsersRoles")]
    public class tblUsersRoles
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public virtual tblUsers? User { get; set; } 
        public virtual tblRoles? Role { get; set; } 
    }
}