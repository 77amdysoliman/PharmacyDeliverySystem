using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Element;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using pharmacy.domin.Identity;
using pharmacy.infrastructuree.Data;

namespace pharmacy.web.Pages.Admin
{
    public class ReportsModel : PageModel
    {
       
            private readonly AppDbContext _db;
            private readonly UserManager<ApplicationUser> _userManager;

            public ReportsModel(AppDbContext db, UserManager<ApplicationUser> userManager)
            {
                _db = db;
                _userManager = userManager;
            }

            public decimal TotalRevenue { get; set; }
            public int TotalOrders { get; set; }
            public double AvgRating { get; set; }

            public int TotalPharmacies { get; set; }
            public int ActivePharmacies { get; set; }

            public int ActiveUsers { get; set; }
            public int NewUsers { get; set; }

            // 🔥 نجيب المستخدمين الطبيعيين زي الداشبورد
            private async Task<HashSet<string>> GetNormalUserIds()
            {
                var allUsers = await _userManager.Users.ToListAsync();

                var normalUsers = new List<ApplicationUser>();

                foreach (var u in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(u);
                    if (roles.Contains("User") || roles.Count == 0)
                        normalUsers.Add(u);
                }

                return normalUsers.Select(u => u.Id).ToHashSet();
            }

            public async Task OnGetAsync()
            {
                var normalUserIds = await GetNormalUserIds();

                TotalOrders = await _db.Orders
                    .AsNoTracking()
                    .CountAsync(o => normalUserIds.Contains(o.UserId));

                TotalRevenue = await _db.Orders
                    .AsNoTracking()
                    .Where(o => normalUserIds.Contains(o.UserId))
                    .SumAsync(o => (decimal?)o.TotalPrice) ?? 0;

                AvgRating = await _db.Pharmacies
                    .AsNoTracking()
                    .AnyAsync()
                    ? await _db.Pharmacies.AsNoTracking().AverageAsync(p => p.Rating)
                    : 0;

                TotalPharmacies = await _db.Pharmacies.AsNoTracking().CountAsync();

                ActivePharmacies = await _db.Pharmacies
                    .AsNoTracking()
                    .CountAsync(p => p.IsOpen);

                ActiveUsers = await _db.Orders
                    .AsNoTracking()
                    .Where(o => normalUserIds.Contains(o.UserId))
                    .Select(o => o.UserId)
                    .Distinct()
                    .CountAsync();

                NewUsers = await _db.User
                    .AsNoTracking()
                    .Where(u => u.CreatedAt >= DateTime.Today.AddDays(-7))
                    .CountAsync();
            }
        }
}