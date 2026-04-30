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

        // 🔥 Download PDF
        public IActionResult OnGetDownload()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Fonts
                var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // Title
                document.Add(new Paragraph("Pharmacy System Report")
                    .SetFont(boldFont)
                    .SetFontSize(20));

                document.Add(new Paragraph($"Date: {DateTime.Now}")
                    .SetFont(normalFont));

                document.Add(new Paragraph("\n"));

                // Section 1
                document.Add(new Paragraph("Revenue & Orders")
                    .SetFont(boldFont));

                document.Add(new Paragraph($"Total Revenue: {TotalRevenue} EGP")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Total Orders: {TotalOrders}")
                    .SetFont(normalFont));

                document.Add(new Paragraph("\n"));

                // Section 2
                document.Add(new Paragraph("Users")
                    .SetFont(boldFont));

                document.Add(new Paragraph($"New Users: {NewUsers}")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Active Users: {ActiveUsers}")
                    .SetFont(normalFont));

                document.Add(new Paragraph("\n"));

                // Section 3
                document.Add(new Paragraph("Pharmacies")
                    .SetFont(boldFont));

                document.Add(new Paragraph($"Total Pharmacies: {TotalPharmacies}")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Active Pharmacies: {ActivePharmacies}")
                    .SetFont(normalFont));

                document.Add(new Paragraph($"Average Rating: {AvgRating}")
                    .SetFont(normalFont));

                document.Close();

                return File(stream.ToArray(), "application/pdf", "Report.pdf");
            }

        }
    }
}