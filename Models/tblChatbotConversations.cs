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
        public int UserID { get; set; }
        public int DocumentID { get; set; }
        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime LastMessageTime { get; set; } = DateTime.Now;
    }
}