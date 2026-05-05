using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.domin.Identity;
using pharmacy.infrastructuree.Data;

namespace pharmacy.web.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardVM Dashboard { get; set; } = new();

        public IndexModel(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var normalUsers = new List<ApplicationUser>();

            foreach (var u in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                if (roles.Contains("User") || roles.Count == 0)
                    normalUsers.Add(u);
            }

            // IDs اليوزر العاديين بس
            var normalUserIds = normalUsers.Select(u => u.Id).ToHashSet();

            Dashboard.TotalUsers = normalUsers.Count;

            // ActiveUsers = اللي عندهم order فعلاً من اليوزر العاديين بس
            Dashboard.ActiveUsers = _db.Orders.AsNoTracking()
                .Where(o => normalUserIds.Contains(o.UserId))
                .Select(o => o.UserId)
                .Distinct()
                .Count();

            Dashboard.TotalPharmacies = _db.Pharmacies.AsNoTracking().Count();

            // TotalOrders = بتاعت اليوزر العاديين بس
            Dashboard.TotalOrders = _db.Orders.AsNoTracking()
                .Count(o => normalUserIds.Contains(o.UserId));

            // TotalRevenue = بتاعت اليوزر العاديين بس
            Dashboard.TotalRevenue = _db.Orders.AsNoTracking().Any(o => normalUserIds.Contains(o.UserId))
                ? _db.Orders.AsNoTracking().Where(o => normalUserIds.Contains(o.UserId)).Sum(o => o.TotalPrice)
                : 0;

            // TodayOrders = بتاعت اليوزر العاديين بس
            Dashboard.TodayOrders = _db.Orders.AsNoTracking()
                .Count(o => o.OrderDate.Date == DateTime.Today && normalUserIds.Contains(o.UserId));

            Dashboard.ActivePharmacies = _db.Pharmacies.AsNoTracking().Count(p => p.IsOpen);

            Dashboard.AvgRating = _db.Pharmacies.AsNoTracking().Any()
                ? _db.Pharmacies.AsNoTracking().Average(p => p.Rating)
                : 0;

            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            Dashboard.RecentActivities = _db.Orders.AsNoTracking()
                .Where(o => normalUserIds.Contains(o.UserId))
                .OrderByDescending(o => o.OrderDate)
                .Take(4)
                .Select(o => new ActivityVM
                {
                    Title = $"New order #{o.Id}",
                    Time = TimeZoneInfo
                        .ConvertTimeFromUtc(o.OrderDate, egyptTimeZone)
                        .ToString("g")
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
            public string Title { get; set; } = "";
            public string Time { get; set; } = "";
        }
    }
}