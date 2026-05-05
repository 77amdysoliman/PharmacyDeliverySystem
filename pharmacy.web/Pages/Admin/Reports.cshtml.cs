using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.infrastructuree.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace pharmacy.web.Pages.Admin
{
    public class ReportsModel : PageModel
    {

        private readonly AppDbContext _db;

        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public double AvgRating { get; set; }

        public int TotalPharmacies { get; set; }
        public int ActivePharmacies { get; set; }

        public int ActiveUsers { get; set; }
        public int NewUsers { get; set; }

        public ReportsModel(AppDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            TotalOrders = _db.Orders.Count();

            TotalRevenue = _db.Orders.Any()
                ? _db.Orders.Sum(o => o.TotalPrice)
                : 0;

            AvgRating = _db.Pharmacies.Any()
                ? _db.Pharmacies.Average(p => p.Rating)
                : 0;

            TotalPharmacies = _db.Pharmacies.Count();

            ActivePharmacies = _db.Pharmacies
                .Count(p => p.Orders.Any());

            ActiveUsers = _db.User
                .Count(u => u.Orders.Any());

            NewUsers = _db.User
                .Count(u => u.CreatedAt >= DateTime.Today.AddDays(-7));
        }


        public IActionResult OnGetDownload()
        {
            // ✅ جيب الداتا هنا مباشرة
            var totalOrders = _db.Orders.Count();
            var totalRevenue = _db.Orders.Any() ? _db.Orders.Sum(o => o.TotalPrice) : 0;
            var avgRating = _db.Pharmacies.Any() ? _db.Pharmacies.Average(p => p.Rating) : 0;
            var totalPharmacies = _db.Pharmacies.Count();
            var activePharmacies = _db.Pharmacies.Count(p => p.Orders.Any());
            var activeUsers = _db.User.Count(u => u.Orders.Any());
            var newUsers = _db.User.Count(u => u.CreatedAt >= DateTime.Today.AddDays(-7));

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                document.Add(new Paragraph("Pharmacy System Report")
                    .SetFont(boldFont).SetFontSize(20));

                document.Add(new Paragraph($"Date: {DateTime.Now}")
                    .SetFont(normalFont));

                document.Add(new Paragraph("\n"));

                // Section 1
                document.Add(new Paragraph("Revenue & Orders")
                    .SetFont(boldFont).SetFontSize(14));

                document.Add(new Paragraph($"Total Revenue: {totalRevenue:N0} EGP")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Total Orders: {totalOrders:N0}")
                    .SetFont(normalFont));

                document.Add(new Paragraph("\n"));

                // Section 2
                document.Add(new Paragraph("Users")
                    .SetFont(boldFont).SetFontSize(14));

                document.Add(new Paragraph($"New Users (last 7 days): {newUsers}")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Active Users: {activeUsers}")
                    .SetFont(normalFont));

                document.Add(new Paragraph("\n"));

                // Section 3
                document.Add(new Paragraph("Pharmacies")
                    .SetFont(boldFont).SetFontSize(14));

                document.Add(new Paragraph($"Total Pharmacies: {totalPharmacies}")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Active Pharmacies: {activePharmacies}")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Average Rating: {avgRating:F1}")
                    .SetFont(normalFont));

                document.Close();

                return File(stream.ToArray(), "application/pdf", "Report.pdf");
            }
        }

    }
    }
