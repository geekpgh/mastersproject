using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewersBuddy.Models
{
    [Table("Cellar")]
    public class Cellar
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CellarId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserProfile Owner { get; set; }

        public virtual ICollection<Container> Containers { get; set; }
    }

}