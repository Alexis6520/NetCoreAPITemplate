using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DemoItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(600)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Price { get; set; }
    }
}
