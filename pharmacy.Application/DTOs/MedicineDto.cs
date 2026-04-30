using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.Application.DTOs
{
    public class MedicineDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Medicine name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 99999, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        public bool RequiresPrescription { get; set; }

        [MaxLength(200, ErrorMessage = "Manufacturer cannot exceed 200 characters")]
        public string? Manufacturer { get; set; }

        public string CategoryName { get; set; } = "";
        public int CategoryId { get; set; }

        public bool IsAvailable { get; set; }
        public int Stock { get; set; }
    }
}
