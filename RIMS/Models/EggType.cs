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
        public int EggTypeId { get; set; }

        public string Name { get; set; }
    }
}
