using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblUserNotifications")]
    public class tblUserNotifications
    {
        [Key]
        public int NotificationID { get; set; }
        public int? UserID { get; set; }
        public string? NotificationType { get; set; }
        public string? NotificationContent { get; set; }
        public int RelatedID { get; set; }
        public string? RelatedType { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ReadDate { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUsers? user { get; set; }

    }
}