using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblChatbotConversations")]
    public class tblChatbotConversations
    {
        [Key]
        public int ConversationID { get; set; }
        public int? UserID { get; set; }
        public int? DocumentID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? LastMessageTime { get; set; }
        [ForeignKey("UserID")]
        public virtual tblUsers? user { get; set; }

        [ForeignKey("DocumentID")]
        public virtual tblDocuments? document { get; set; }

    }
}