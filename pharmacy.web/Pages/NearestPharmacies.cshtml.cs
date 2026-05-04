using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.domin.Entites;
using pharmacy.domin.Identity;
using pharmacy.infrastructuree.Data;


namespace Pharmacy.Web.Pages
{
    [Authorize]
    public class NearestPharmaciesModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NearestPharmaciesModel(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<(pharmacy.domin.Entites.Pharmacy Pharmacy, double Distance)> Pharmacies { get; set; } = new();

        public async Task OnGetAsync()
        {
            // جيب موقع اليوزر من الـ Database
            var user = await _userManager.GetUserAsync(User);

            double userLat = user?.Latitude ?? 30.0444;  // Cairo default لو مفيش موقع
            double userLng = user?.Longitude ?? 31.2357;

            var all = await _context.Pharmacies.ToListAsync();

            Pharmacies = all
                .Select(p => (p, CalcDistance(userLat, userLng, p.Latitude, p.Longitude)))
                .OrderBy(x => x.Item2)
                .ToList();
        }

        private static double CalcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return Math.Round(6371 * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)), 1);
        }
    }
}