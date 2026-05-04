using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.domin.Identity
{
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // لو كان Pharmacy Admin
        public int? PharmacyId { get; set; }
    }
}
