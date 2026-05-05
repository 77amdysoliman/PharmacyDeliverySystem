using System.Text.Json;

namespace pharmacy.web.Pages.Helpers
{
    public class CartItem
    {
        public int MedicineId { get; set; } 

        public string MedicineName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }

    public static class CartHelper
    {
        private const string CartKey = "Cart";

        public static List<CartItem> GetCart(ISession session)
        {
            var json = session.GetString(CartKey);
            return json == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(json)!;
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        public static void AddItem(ISession session, CartItem item)
        {
            var cart = GetCart(session);
            var existing = cart.FirstOrDefault(x => x.MedicineName == item.MedicineName);
            if (existing != null)
                existing.Quantity += item.Quantity;
            else
                cart.Add(item);
            SaveCart(session, cart);
        }

        public static void RemoveItem(ISession session, string medicineName)
        {
            var cart = GetCart(session);
            cart.RemoveAll(x => x.MedicineName == medicineName);
            SaveCart(session, cart);
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartKey);
        }
    }
}