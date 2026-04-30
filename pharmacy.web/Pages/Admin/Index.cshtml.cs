using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using pharmacy.infrastructuree.Data;
namespace pharmacy.web.Pages.Admin
{
    public class IndexModel : PageModel
    {

        private readonly AppDbContext _db;

        public DashboardVM Dashboard { get; set; } = new();

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Dashboard.TotalUsers = _db.User.Count(); // ✅
            Dashboard.TotalPharmacies = _db.Pharmacies.Count();
            Dashboard.TotalOrders = _db.Orders.Count();

            Dashboard.TotalRevenue = _db.Orders.Any()
                ? _db.Orders.Sum(o => o.TotalPrice)
                : 0;

            Dashboard.TodayOrders = _db.Orders
                  .Count(o => o.OrderDate.Date == DateTime.Today);

            Dashboard.ActiveUsers = _db.User
                .Count(u => u.Orders.Any()); // ✅

            Dashboard.ActivePharmacies = _db.Pharmacies
                .Count(p => p.Orders.Any());

            Dashboard.AvgRating = _db.Pharmacies.Any()
                ? _db.Pharmacies.Average(p => p.Rating)
                : 0;

            Dashboard.RecentActivities = _db.Orders
     .OrderByDescending(o => o.OrderDate)
     .Take(4)
     .Select(o => new ActivityVM
     {
         Title = $"New order #{o.Id}",
         Time = o.OrderDate.ToString("g")
     }).ToList();
        }

        public class DashboardVM
        {
            public int TotalUsers { get; set; }
            public int TotalPharmacies { get; set; }
            public int TotalOrders { get; set; }
            public decimal TotalRevenue { get; set; }

            public int TodayOrders { get; set; }
            public int ActiveUsers { get; set; }
            public int ActivePharmacies { get; set; }
            public double AvgRating { get; set; }

            public List<ActivityVM> RecentActivities { get; set; } = new();
        }

        public class ActivityVM
        {
            public string Title { get; set; }
            public string Time { get; set; }
        }
    }
}
