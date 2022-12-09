using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCKGRND.Models
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "MEDIUMBLOB")]
        public string? Image { get; set; }
        
        public virtual Location? Location { get; set; }
        
    }
}
