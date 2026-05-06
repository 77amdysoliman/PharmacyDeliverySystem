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

        public IndexModel(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal Total => CartItems.Sum(x => x.Total);

        [BindProperty(SupportsGet = true)]
        public int PharmacyId { get; set; }

        [BindProperty]
        public string DeliveryAddress { get; set; } = "";

        [BindProperty]
        public string? Notes { get; set; }

        public async Task OnGetAsync()
        {
            CartItems = CartHelper.GetCart(HttpContext.Session);
            if (PharmacyId == 0)
                PharmacyId = HttpContext.Session.GetInt32("PharmacyId") ?? 0;
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
                DeliveryAddress = user.Address ?? "";
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
            if (PharmacyId == 0)
                PharmacyId = HttpContext.Session.GetInt32("PharmacyId") ?? 0;

            var orderDto = new OrderDto
            {
                UserId = user.Id,
                PharmacyId = PharmacyId,
                DeliveryAddress = DeliveryAddress,
                Notes = Notes,
                Items = cart.Select(x => new OrderItemDto
                {
                    MedicineId = x.MedicineId,
                    MedicineName = x.MedicineName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                }).ToList()
            };

            // ? «·”ÿ— œÂ ﬂ«‰ ‰«ﬁ’!
            await _orderService.CreateOrderAsync(orderDto);

            CartHelper.ClearCart(HttpContext.Session);
            HttpContext.Session.Remove("PharmacyId");

            return RedirectToPage("/MyOrders/Index");
        }
    }
}