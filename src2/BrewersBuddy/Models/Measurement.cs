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

        [Display(Name = "Date")]
        public DateTime MeasurementDate { get; set; }

        public string Description { get; set; }

        public double Value { get; set; }

        //This is what was measured, such as SO2, ABV, gravity
        public String Measured { get; set; }

        [Display(Name = "Batch")]
        public int BatchId { get; set; }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }

        public virtual String SummaryText
        {
            get
            {
                return this.Description.Length > 200 ? this.Description.Substring(0, 200) + "..." :
                       this.Description;
            }
        }
    }

}