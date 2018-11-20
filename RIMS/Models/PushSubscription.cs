using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models
{
    public class PushSubscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        public string PushAuth { get; set; }
    }
}
