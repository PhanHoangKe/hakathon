using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblRoles")]
    public class tblRoles
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public virtual ICollection<tblUsersRoles> UserRoles { get; set; } = new List<tblUsersRoles>();
    }
}