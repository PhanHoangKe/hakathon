using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
    [Table("tblChatbotMessages")]
    public class tblChatbotMessages
    {
        [Key]
        public int MessageID { get; set; }
        public int? ConversationID { get; set; }
        public bool? IsUserMessage { get; set; }
        public string? MessageText { get; set; }
        public DateTime? MessageTime { get; set; }


    }
}