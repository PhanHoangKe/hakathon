using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblContactMessages")]
    public class tblContactMessages
    {
        [Key]
        public int MessageID { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public int? UserID { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsReplied { get; set; } = false;
        public DateTime MessageDate { get; set; } = DateTime.Now;
    }
}