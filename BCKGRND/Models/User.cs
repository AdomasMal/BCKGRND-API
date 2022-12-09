using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCKGRND.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "nvarChar(100)")]
        public string? UserMail { get; set; }

        [Column(TypeName = "nvarChar(30)")]
        public string? UserName { get; set; }

        [Column(TypeName = "nvarChar(100)")]
        public string? UserPass { get; set; }

        [Column(TypeName = "nvarChar(30)")]
        public string? Salt { get; set; }
    }
}
