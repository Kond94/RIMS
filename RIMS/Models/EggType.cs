using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models
{
    [Table("EggTypes")]
    public class EggType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte CandlingDays { get; set; }

        public byte HatchPreparationDays { get; set; }

        public byte HatchDays { get; set; }
    }

    public enum EggTypes : int
    {
        None = 1,
        Quail = 2,
        Chicken = 3,
        Duck = 4,
        Turkey = 5,
        Guinea_Fowl = 6       
    }
}
