using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
using System.ComponentModel.DataAnnotations.Schema;

[Table("Emails")]
public class Emails
{
    public int EmailsId { get; set; } // Thêm thuộc tính EmailsId làm khóa chính
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPass { get; set; }
}
}