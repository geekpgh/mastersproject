﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BrewersBuddy.Models
{
    [Table("Container")]
    public class Container
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ContainerId { get; set; }
        public string Name { get; set; }
        public double Volume { get; set; }
        public ContainerVolumeUnits Unit { get; set; }
        
        public Batch Batch { get; set; }

        [Required]
        public int  ContainerTypeValue { get; set; }

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
    }

}