using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hakathon.Models
{
  [Table("tblAuthors")]
  public class tblAuthors
  {
    [Key]
    public int AuthorID { get; set; }
    public string? AuthorName { get; set; }
    public string? Biography { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
   public class Author
{
    public int AuthorID { get; set; }
    public string AuthorName { get; set; }
    public string? Biography { get; set; }
    public string? Email { get; set; }


}

  }
}