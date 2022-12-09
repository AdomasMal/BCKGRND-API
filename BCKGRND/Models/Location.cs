using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCKGRND.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "nvarChar(50)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarChar(200)")]
        public string? Description { get; set; }

        [Column(TypeName = "FLOAT(9, 7)")]
        public float Latitude { get; set; }

        [Column(TypeName = "FLOAT(10, 7)")]
        public float Longtitude { get; set; }

        public virtual ICollection<Tag>? Tags { get; set; }

        public virtual ICollection<Photo>? Photos { get; set; }
    }
}
