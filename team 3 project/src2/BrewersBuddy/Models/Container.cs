using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewersBuddy.Models
{
    [Table("Container")]
    public class Container
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ContainerId { get; set; }
        [Required]
        public string Name { get; set; }
        public double Volume { get; set; }
        public int UnitValue { get; set; }
        public int BatchId { get; set; }
        public int OwnerId { get; set; }
        public int Quantity { get; set; }

        [Required]
        public int ContainerTypeValue { get; set; }

        public ContainerType Type
        {
            get
            {
                return (ContainerType)Enum.ToObject(typeof(ContainerType), ContainerTypeValue);
            }
            set
            {
                ContainerTypeValue = (int)value;
            }
        }


        public ContainerVolumeUnits Units
        {
            get
            {
                return (ContainerVolumeUnits)Enum.ToObject(typeof(ContainerVolumeUnits), UnitValue);
            }
            set
            {
                UnitValue = (int)value;
            }
        }

        [ForeignKey("BatchId")]
        public virtual Batch Batch { get; set; }


        public bool CanView(int userId)
        {
            Batch batch = this.Batch;
            return batch.CanView(userId);
        }


        public bool CanEdit(int userId)
        {
            Batch batch = this.Batch;
            return batch.CanEdit(userId);
        }
    }

}