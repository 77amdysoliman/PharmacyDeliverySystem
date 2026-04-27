using pharmacy.domin.Entites;

namespace pharmacy.infrastructuree.Data
{
    public static class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Antibiotics", Description = "Kill or inhibit bacteria" },
                    new Category { Name = "Painkillers", Description = "Relieve pain" },
                    new Category { Name = "Vitamins", Description = "Nutritional supplements" },
                    new Category { Name = "Antidiabetics", Description = "Control blood sugar" },
                    new Category { Name = "Heart Medicines", Description = "Treat heart conditions" },
                    new Category { Name = "Allergy", Description = "Treat allergic reactions" },
                    new Category { Name = "Stomach", Description = "Treat digestive issues" },
                    new Category { Name = "Skin Care", Description = "Treat skin conditions" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Medicines.Any())
            {
                var medicines = new List<Medicine>
                {
                    // Antibiotics
                    new Medicine { Name = "Amoxicillin 500mg", Price = 25.50m, CategoryId = 1, Manufacturer = "GlaxoSmithKline", RequiresPrescription = true },
                    new Medicine { Name = "Azithromycin 250mg", Price = 45.00m, CategoryId = 1, Manufacturer = "Pfizer", RequiresPrescription = true },
                    new Medicine { Name = "Ciprofloxacin 500mg", Price = 30.00m, CategoryId = 1, Manufacturer = "Bayer", RequiresPrescription = true },

                    // Painkillers
                    new Medicine { Name = "Paracetamol 500mg", Price = 10.00m, CategoryId = 2, Manufacturer = "Novartis", RequiresPrescription = false },
                    new Medicine { Name = "Ibuprofen 400mg", Price = 15.00m, CategoryId = 2, Manufacturer = "Sanofi", RequiresPrescription = false },
                    new Medicine { Name = "Aspirin 100mg", Price = 8.00m, CategoryId = 2, Manufacturer = "Bayer", RequiresPrescription = false },

                    // Vitamins
                    new Medicine { Name = "Vitamin C 1000mg", Price = 20.00m, CategoryId = 3, Manufacturer = "Pharco", RequiresPrescription = false },
                    new Medicine { Name = "Vitamin D3 1000IU", Price = 35.00m, CategoryId = 3, Manufacturer = "Novartis", RequiresPrescription = false },
                    new Medicine { Name = "Zinc 50mg", Price = 18.00m, CategoryId = 3, Manufacturer = "Pharco", RequiresPrescription = false },

                    // Antidiabetics
                    new Medicine { Name = "Metformin 500mg", Price = 12.00m, CategoryId = 4, Manufacturer = "Merck", RequiresPrescription = true },
                    new Medicine { Name = "Glibenclamide 5mg", Price = 22.00m, CategoryId = 4, Manufacturer = "Sanofi", RequiresPrescription = true },

                    // Heart
                    new Medicine { Name = "Atorvastatin 20mg", Price = 40.00m, CategoryId = 5, Manufacturer = "Pfizer", RequiresPrescription = true },
                    new Medicine { Name = "Amlodipine 5mg", Price = 28.00m, CategoryId = 5, Manufacturer = "Pfizer", RequiresPrescription = true },

                    // Allergy
                    new Medicine { Name = "Cetirizine 10mg", Price = 14.00m, CategoryId = 6, Manufacturer = "UCB", RequiresPrescription = false },
                    new Medicine { Name = "Loratadine 10mg", Price = 16.00m, CategoryId = 6, Manufacturer = "Schering", RequiresPrescription = false },

                    // Stomach
                    new Medicine { Name = "Omeprazole 20mg", Price = 22.00m, CategoryId = 7, Manufacturer = "AstraZeneca", RequiresPrescription = false },
                    new Medicine { Name = "Domperidone 10mg", Price = 18.00m, CategoryId = 7, Manufacturer = "Janssen", RequiresPrescription = false },

                    // Skin
                    new Medicine { Name = "Hydrocortisone Cream 1%", Price = 25.00m, CategoryId = 8, Manufacturer = "GSK", RequiresPrescription = false },
                    new Medicine { Name = "Clotrimazole Cream 1%", Price = 20.00m, CategoryId = 8, Manufacturer = "Bayer", RequiresPrescription = false },
                };
                await context.Medicines.AddRangeAsync(medicines);
                await context.SaveChangesAsync();
            }

            if (!context.Pharmacies.Any())
            {
                var pharmacies = new List<Pharmacy>
                {
                    new Pharmacy { Name = "El Ezaby Pharmacy", Address = "123 Tahrir Square, Cairo", Phone = "0201234567", Email = "elezaby@pharmacy.com", Latitude = 30.0444, Longitude = 31.2357, Rating = 4.8, IsOpen = true, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                    new Pharmacy { Name = "Seif Pharmacy", Address = "45 Mohandiseen St, Giza", Phone = "0209876543", Email = "seif@pharmacy.com", Latitude = 30.0580, Longitude = 31.2100, Rating = 4.6, IsOpen = true, OpenTime = new TimeSpan(9, 0, 0), CloseTime = new TimeSpan(22, 0, 0) },
                    new Pharmacy { Name = "El Dawaa Pharmacy", Address = "78 Heliopolis, Cairo", Phone = "0201122334", Email = "eldawaa@pharmacy.com", Latitude = 30.0900, Longitude = 31.3200, Rating = 4.5, IsOpen = true, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(23, 59, 0) },   
                    new Pharmacy { Name = "Ramses Pharmacy", Address = "12 Ramses St, Cairo", Phone = "0205544332", Email = "ramses@pharmacy.com", Latitude = 30.0650, Longitude = 31.2500, Rating = 4.3, IsOpen = false, OpenTime = new TimeSpan(10, 0, 0), CloseTime = new TimeSpan(21, 0, 0) },
                    new Pharmacy { Name = "Zamalek Pharmacy", Address = "9 Zamalek, Cairo", Phone = "0207788990", Email = "zamalek@pharmacy.com", Latitude = 30.0600, Longitude = 31.2200, Rating = 4.7, IsOpen = true, OpenTime = new TimeSpan(8, 0, 0), CloseTime = new TimeSpan(23, 0, 0) },
                };
                await context.Pharmacies.AddRangeAsync(pharmacies);
                await context.SaveChangesAsync();
            }

            if (!context.PharmacyMedicines.Any())
            {
                var pharmacyMedicines = new List<PharmacyMedicine>
                {
                    new PharmacyMedicine { PharmacyId = 1, MedicineId = 1, Stock = 50, Price = 26.00m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 1, MedicineId = 2, Stock = 30, Price = 46.00m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 1, MedicineId = 4, Stock = 100, Price = 10.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 1, MedicineId = 7, Stock = 60, Price = 21.00m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 2, MedicineId = 3, Stock = 40, Price = 31.00m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 2, MedicineId = 5, Stock = 80, Price = 15.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 2, MedicineId = 10, Stock = 25, Price = 12.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 3, MedicineId = 6, Stock = 90, Price = 8.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 3, MedicineId = 8, Stock = 45, Price = 36.00m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 3, MedicineId = 12, Stock = 20, Price = 41.00m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 4, MedicineId = 14, Stock = 70, Price = 14.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 4, MedicineId = 16, Stock = 55, Price = 22.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 5, MedicineId = 17, Stock = 35, Price = 18.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 5, MedicineId = 18, Stock = 40, Price = 25.50m, IsAvailable = true },
                    new PharmacyMedicine { PharmacyId = 5, MedicineId = 19, Stock = 30, Price = 20.50m, IsAvailable = true },
                };
                await context.PharmacyMedicines.AddRangeAsync(pharmacyMedicines);
                await context.SaveChangesAsync();
            }

            if (!context.User.Any())
            {
                var users = new List<User>
                {
                    new User { FullName = "Ahmed Mohamed", Email = "ahmed@gmail.com", Phone = "01012345678", Address = "Cairo", Latitude = 30.0444, Longitude = 31.2357 },
                    new User { FullName = "Sara Ali", Email = "sara@gmail.com", Phone = "01098765432", Address = "Giza", Latitude = 30.0580, Longitude = 31.2100 },
                    new User { FullName = "Mohamed Hassan", Email = "mohamed@gmail.com", Phone = "01112233445", Address = "Heliopolis", Latitude = 30.0900, Longitude = 31.3200 },
                    new User { FullName = "Nour Khaled", Email = "nour@gmail.com", Phone = "01567890123", Address = "Zamalek", Latitude = 30.0600, Longitude = 31.2200 },
                    new User { FullName = "Omar Tarek", Email = "omar@gmail.com", Phone = "01234567890", Address = "Mohandiseen", Latitude = 30.0650, Longitude = 31.2500 },
                };
                await context.User.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }
        }
    }
}