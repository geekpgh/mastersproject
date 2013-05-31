using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewersBuddy.Models
{
    [Table("Measurement")]
    public class Measurement
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MeasurementId { get; set; }
        public string Name { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        //This is what was measured, such as SO2, ABV, gravity
        public String Measured { get; set; }
        public Batch Batch { get; set; }
    }

}