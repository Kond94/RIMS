using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RIMS.Models
{
    public class PushSubscription
    {
        public int Id { get; set; }
        [Required]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        public string PushAuth { get; set; }
    }
}
