using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.Application.DTOs
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool RequiresPrescription { get; set; }
        public string? Manufacturer { get; set; }
        public string CategoryName { get; set; }
        public bool IsAvailable { get; set; }
        public int Stock { get; set; }
    }
}
