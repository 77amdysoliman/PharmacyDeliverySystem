using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
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
        public int TotalUsers { get; set; }      // ✅ كل اليوزرز
        public int NewUsers { get; set; }         // ✅ اليوزرز الجدد آخر 7 أيام

        // ✅ بيجيب كل اليوزرز العاديين (role = "User" أو بدون role)
        private async Task<List<ApplicationUser>> GetNormalUsers()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var normalUsers = new List<ApplicationUser>();

            foreach (var u in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(u);
                if (roles.Contains("User") || roles.Count == 0)
                    normalUsers.Add(u);
            }

            return normalUsers;
        }

        private async Task LoadStatsAsync()
        {
            var normalUsers = await GetNormalUsers();
            var normalUserIds = normalUsers.Select(u => u.Id).ToHashSet();

            TotalOrders = await _db.Orders
                .AsNoTracking()
                .CountAsync(o => normalUserIds.Contains(o.UserId));

            TotalRevenue = await _db.Orders
                .AsNoTracking()
                .Where(o => normalUserIds.Contains(o.UserId))
                .SumAsync(o => (decimal?)o.TotalPrice) ?? 0;

            AvgRating = await _db.Pharmacies.AsNoTracking().AnyAsync()
                ? await _db.Pharmacies.AsNoTracking().AverageAsync(p => p.Rating)
                : 0;

            TotalPharmacies = await _db.Pharmacies.AsNoTracking().CountAsync();

            ActivePharmacies = await _db.Pharmacies
                .AsNoTracking()
                .CountAsync(p => p.IsOpen);

            
            TotalUsers =  normalUsers.Count;

            NewUsers = await _db.Orders
     .AsNoTracking()
     .Where(o => normalUserIds.Contains(o.UserId))
     .Select(o => o.UserId)
     .Distinct()
     .CountAsync();
            //active users 

        }

        public async Task OnGetAsync()
        {
            await LoadStatsAsync();
        }

        public async Task<IActionResult> OnGetDownloadAsync()
        {
            await LoadStatsAsync();

            byte[] pdfBytes;

            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
                document.SetMargins(40, 40, 40, 40);

                var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                var headerColor = new iText.Kernel.Colors.DeviceRgb(74, 108, 247);
                var lightBg = new iText.Kernel.Colors.DeviceRgb(245, 247, 255);
                var darkText = new iText.Kernel.Colors.DeviceRgb(26, 26, 46);
                var greenColor = new iText.Kernel.Colors.DeviceRgb(39, 174, 96);
                var mutedColor = new iText.Kernel.Colors.DeviceRgb(100, 100, 120);
                var white = iText.Kernel.Colors.ColorConstants.WHITE;

                // ─── Header Banner ───
                var headerTable = new Table(new float[] { 515 })
                    .SetWidth(515).SetMarginBottom(24);

                var headerCell = new Cell()
                    .SetBackgroundColor(headerColor)
                    .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                    .SetPaddingLeft(20).SetPaddingRight(20)
                    .SetPaddingTop(18).SetPaddingBottom(14);

                headerCell.Add(new Paragraph("Pharmacy System — Full Report")
                    .SetFont(boldFont).SetFontSize(18)
                    .SetFontColor(white).SetMarginBottom(4));

                headerCell.Add(new Paragraph($"Generated: {DateTime.Now:dd MMM yyyy   HH:mm}")
                    .SetFont(normalFont).SetFontSize(10)
                    .SetFontColor(new iText.Kernel.Colors.DeviceRgb(190, 200, 255))
                    .SetMarginBottom(0));

                headerTable.AddCell(headerCell);
                document.Add(headerTable);

                // ─── Helpers ───
                void AddSection(string title)
                {
                    document.Add(new Paragraph(title)
                        .SetFont(boldFont).SetFontSize(13)
                        .SetFontColor(headerColor)
                        .SetMarginTop(16).SetMarginBottom(2));

                    var line = new Table(new float[] { 515 }).SetWidth(515).SetMarginBottom(8);
                    line.AddCell(new Cell()
                        .SetHeight(2).SetBackgroundColor(headerColor)
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    document.Add(line);
                }

                void AddRow(string label, string value, bool highlight = false)
                {
                    var row = new Table(new float[] { 310, 205 })
                        .SetWidth(515).SetMarginBottom(3);

                    row.AddCell(new Cell()
                        .Add(new Paragraph(label)
                            .SetFont(normalFont).SetFontSize(10)
                            .SetFontColor(mutedColor))
                        .SetBackgroundColor(lightBg)
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                        .SetPaddingLeft(12).SetPaddingTop(9).SetPaddingBottom(9));

                    row.AddCell(new Cell()
                        .Add(new Paragraph(value)
                            .SetFont(boldFont).SetFontSize(11)
                            .SetFontColor(highlight ? greenColor : darkText))
                        .SetBackgroundColor(lightBg)
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                        .SetPaddingRight(12).SetPaddingTop(9).SetPaddingBottom(9)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                    document.Add(row);
                }

                AddSection("Revenue & Orders");
                AddRow("Total Revenue", $"{TotalRevenue:N0} EGP", highlight: true);
                AddRow("Total Orders", $"{TotalOrders:N0}");
                AddRow("Estimated Growth", "+15%", highlight: true);

                AddSection("Users");
                
                AddRow("Total Users", $"{TotalUsers:N0}");
                AddRow("Active Users ", $"{NewUsers:N0}");
                AddRow("Retention Rate", "78%");

                AddSection("Pharmacies");
                AddRow("Total Pharmacies", $"{TotalPharmacies:N0}");
                AddRow("Active Pharmacies", $"{ActivePharmacies:N0}", highlight: true);
                AddRow("Average Rating", $"{AvgRating:F1} / 5.0");

                document.Add(new Paragraph("\n\n"));
                document.Add(new Paragraph("This report is auto-generated by the Pharmacy Management System.")
                    .SetFont(normalFont).SetFontSize(8)
                    .SetFontColor(new iText.Kernel.Colors.DeviceRgb(180, 180, 200))
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                document.Close();
                pdfBytes = ms.ToArray();
            }

            return File(pdfBytes, "application/pdf", $"Report_{DateTime.Today:yyyy-MM-dd}.pdf");
        }
    }
}