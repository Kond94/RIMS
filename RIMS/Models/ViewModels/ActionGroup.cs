using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models.ViewModels
{
    public class ActionGroup
    {
        public int Id { get; set; }
        public int[] TrayIds { get; set; }

        public DateTime ActionDate { get; set; }
    }
}
