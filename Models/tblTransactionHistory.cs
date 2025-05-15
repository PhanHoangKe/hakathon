using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblTransactionHistory")]
    public class tblTransactionHistory
    {
        [Key]
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public int? DocumentID { get; set; }
        [Required]
        public string ActionType { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public string? Description { get; set; }
    }
}