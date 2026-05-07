using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pharmacy.Application.DTOs;
using pharmacy.Application.Interfaces;
using pharmacy.domin.Identity;
using pharmacy.web.Pages.Helpers;

namespace pharamcy.web.Pages.Cart
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public IndexModel(IOrderService orderService,
                         UserManager<ApplicationUser> userManager,
                         IWebHostEnvironment env)
        {
            _orderService = orderService;
            _userManager = userManager;
            _env = env;
        }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal Total => CartItems.Sum(x => x.Total);

        [BindProperty(SupportsGet = true)]
        public int PharmacyId { get; set; }

        [BindProperty]
        public string DeliveryAddress { get; set; } = "";

        [BindProperty]
        public string? Notes { get; set; }

        // Prescription
        [BindProperty]
        public IFormFile? PrescriptionFile { get; set; }

        public bool HasPrescriptionInSession { get; set; }

        public async Task OnGetAsync()
        {
            CartItems = CartHelper.GetCart(HttpContext.Session);
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                DeliveryAddress = user.Address ?? "";

            // ‘Ê› ·Ê ›Ì prescription ›Ì «·‹ Session
            HasPrescriptionInSession = !string.IsNullOrEmpty(
                HttpContext.Session.GetString("PrescriptionImageBase64"));
        }

        public IActionResult OnPostRemove(string medicineName)
        {
            CartHelper.RemoveItem(HttpContext.Session, medicineName);
            return RedirectToPage(new { PharmacyId });
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("/Account/Login");

            var cart = CartHelper.GetCart(HttpContext.Session);
            if (!cart.Any()) return Page();

            // ? «Õ›Ÿ «·’Ê—… ·Ê „ÊÃÊœ…
            string? prescriptionImagePath = null;

            var base64 = HttpContext.Session.GetString("PrescriptionImageBase64");
            var fileName = HttpContext.Session.GetString("PrescriptionFileName");

            if (!string.IsNullOrEmpty(base64) && !string.IsNullOrEmpty(fileName))
            {
                // ÕÊ¯· Base64 ·‹ bytes Ê«Õ›ŸÂ«
                var bytes = Convert.FromBase64String(base64.Split(',')[1]);
                var uniqueName = $"{Guid.NewGuid()}_{fileName}";
                var folder = Path.Combine(_env.WebRootPath, "prescriptions");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, uniqueName);
                await System.IO.File.WriteAllBytesAsync(filePath, bytes);

                prescriptionImagePath = $"/prescriptions/{uniqueName}";

                // «„”Õ „‰ «·‹ Session
                HttpContext.Session.Remove("PrescriptionImageBase64");
                HttpContext.Session.Remove("PrescriptionFileName");
            }

            var orderDto = new OrderDto
            {
                UserId = user.Id,
                PharmacyId = PharmacyId,
                DeliveryAddress = DeliveryAddress,
                Notes = Notes,
                PrescriptionImagePath = prescriptionImagePath, // ?
                Items = cart.Select(x => new OrderItemDto
                {
                    MedicineId = x.MedicineId,
                    MedicineName = x.MedicineName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                }).ToList()
            };

            await _orderService.CreateOrderAsync(orderDto);
            CartHelper.ClearCart(HttpContext.Session);

            return RedirectToPage("/MyOrders/Index");
        }
    }
}