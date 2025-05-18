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
        public int? UserID { get; set; }
        public int? DocumentID { get; set; }
        public string? ActionType { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Description { get; set; }
       [ForeignKey("UserID")]
        public virtual tblUsers? user { get; set; }

        [ForeignKey("DocumentID")]
        public virtual tblDocuments? document { get; set; }



    }
}