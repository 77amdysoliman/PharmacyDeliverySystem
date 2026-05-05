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

            var existingMedicineNames = context.Medicines.Select(m => m.Name).ToList();

            var allMedicines = new List<Medicine>
{
    // Antibiotics
    new Medicine { Name = "Amoxicillin 500mg", Price = 25.50m, CategoryId = 1, Manufacturer = "GlaxoSmithKline", RequiresPrescription = true },
    new Medicine { Name = "Azithromycin 250mg", Price = 45.00m, CategoryId = 1, Manufacturer = "Pfizer", RequiresPrescription = true },
    new Medicine { Name = "Ciprofloxacin 500mg", Price = 30.00m, CategoryId = 1, Manufacturer = "Bayer", RequiresPrescription = true },
    new Medicine { Name = "Amoxicillin 250mg", Price = 18.00m, CategoryId = 1, Manufacturer = "Pharco", RequiresPrescription = true },
    new Medicine { Name = "Doxycycline 100mg", Price = 35.00m, CategoryId = 1, Manufacturer = "Pfizer", RequiresPrescription = true },

    // Painkillers
    new Medicine { Name = "Paracetamol 500mg", Price = 10.00m, CategoryId = 2, Manufacturer = "Novartis", RequiresPrescription = false },
    new Medicine { Name = "Ibuprofen 400mg", Price = 15.00m, CategoryId = 2, Manufacturer = "Sanofi", RequiresPrescription = false },
    new Medicine { Name = "Aspirin 100mg", Price = 8.00m, CategoryId = 2, Manufacturer = "Bayer", RequiresPrescription = false },
    new Medicine { Name = "Paracetamol 250mg Syrup", Price = 12.00m, CategoryId = 2, Manufacturer = "Novartis", RequiresPrescription = false },
    new Medicine { Name = "Diclofenac 50mg", Price = 20.00m, CategoryId = 2, Manufacturer = "Novartis", RequiresPrescription = false },

    // Vitamins
    new Medicine { Name = "Vitamin C 1000mg", Price = 20.00m, CategoryId = 3, Manufacturer = "Pharco", RequiresPrescription = false },
    new Medicine { Name = "Vitamin D3 1000IU", Price = 35.00m, CategoryId = 3, Manufacturer = "Novartis", RequiresPrescription = false },
    new Medicine { Name = "Zinc 50mg", Price = 18.00m, CategoryId = 3, Manufacturer = "Pharco", RequiresPrescription = false },
    new Medicine { Name = "Vitamin B12 1000mcg", Price = 28.00m, CategoryId = 3, Manufacturer = "Pharco", RequiresPrescription = false },
    new Medicine { Name = "Omega 3 1000mg", Price = 45.00m, CategoryId = 3, Manufacturer = "GSK", RequiresPrescription = false },
    new Medicine { Name = "Iron 65mg", Price = 15.00m, CategoryId = 3, Manufacturer = "Sanofi", RequiresPrescription = false },

    // Antidiabetics
    new Medicine { Name = "Metformin 500mg", Price = 12.00m, CategoryId = 4, Manufacturer = "Merck", RequiresPrescription = true },
    new Medicine { Name = "Glibenclamide 5mg", Price = 22.00m, CategoryId = 4, Manufacturer = "Sanofi", RequiresPrescription = true },
    new Medicine { Name = "Sitagliptin 100mg", Price = 85.00m, CategoryId = 4, Manufacturer = "Merck", RequiresPrescription = true },

    // Heart
    new Medicine { Name = "Atorvastatin 20mg", Price = 40.00m, CategoryId = 5, Manufacturer = "Pfizer", RequiresPrescription = true },
    new Medicine { Name = "Amlodipine 5mg", Price = 28.00m, CategoryId = 5, Manufacturer = "Pfizer", RequiresPrescription = true },
    new Medicine { Name = "Aspirin 81mg (Cardiac)", Price = 10.00m, CategoryId = 5, Manufacturer = "Bayer", RequiresPrescription = true },
    new Medicine { Name = "Bisoprolol 5mg", Price = 32.00m, CategoryId = 5, Manufacturer = "Merck", RequiresPrescription = true },

    // Allergy
    new Medicine { Name = "Cetirizine 10mg", Price = 14.00m, CategoryId = 6, Manufacturer = "UCB", RequiresPrescription = false },
    new Medicine { Name = "Loratadine 10mg", Price = 16.00m, CategoryId = 6, Manufacturer = "Schering", RequiresPrescription = false },
    new Medicine { Name = "Desloratadine 5mg", Price = 25.00m, CategoryId = 6, Manufacturer = "Schering", RequiresPrescription = false },
    new Medicine { Name = "Montelukast 10mg", Price = 40.00m, CategoryId = 6, Manufacturer = "Sanofi", RequiresPrescription = false },

    // Stomach
    new Medicine { Name = "Omeprazole 20mg", Price = 22.00m, CategoryId = 7, Manufacturer = "AstraZeneca", RequiresPrescription = false },
    new Medicine { Name = "Domperidone 10mg", Price = 18.00m, CategoryId = 7, Manufacturer = "Janssen", RequiresPrescription = false },
    new Medicine { Name = "Pantoprazole 40mg", Price = 28.00m, CategoryId = 7, Manufacturer = "Pfizer", RequiresPrescription = false },
    new Medicine { Name = "Metoclopramide 10mg", Price = 8.00m, CategoryId = 7, Manufacturer = "Sanofi", RequiresPrescription = false },

    // Skin
    new Medicine { Name = "Hydrocortisone Cream 1%", Price = 25.00m, CategoryId = 8, Manufacturer = "GSK", RequiresPrescription = false },
    new Medicine { Name = "Clotrimazole Cream 1%", Price = 20.00m, CategoryId = 8, Manufacturer = "Bayer", RequiresPrescription = false },
    new Medicine { Name = "Betamethasone Cream 0.1%", Price = 22.00m, CategoryId = 8, Manufacturer = "GSK", RequiresPrescription = false },
    new Medicine { Name = "Mupirocin Ointment 2%", Price = 35.00m, CategoryId = 8, Manufacturer = "GSK", RequiresPrescription = false },
};

            var medicinesToAdd = allMedicines
                .Where(m => !existingMedicineNames.Contains(m.Name))
                .ToList();

            if (medicinesToAdd.Any())
            {
                await context.Medicines.AddRangeAsync(medicinesToAdd);
                await context.SaveChangesAsync(); // ✅ خلاص الأول
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

            // ✅ جيب الـ IDs الحقيقية من الـ Database بعد الـ SaveChanges
            var medicineIds = context.Medicines
        .Select(m => new { m.Id, m.Name })
        .GroupBy(m => m.Name)
        .ToDictionary(g => g.Key, g => g.First().Id);

            var existingPharmacyMedicines = context.PharmacyMedicines
                .Select(pm => new { pm.PharmacyId, pm.MedicineId })
                .ToList();

            var allPharmacyMedicines = new List<PharmacyMedicine>
{
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Amoxicillin 500mg"], Stock = 50, Price = 26.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Azithromycin 250mg"], Stock = 30, Price = 46.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Paracetamol 500mg"], Stock = 100, Price = 10.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Vitamin C 1000mg"], Stock = 60, Price = 21.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 2, MedicineId = medicineIds["Ciprofloxacin 500mg"], Stock = 40, Price = 31.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 2, MedicineId = medicineIds["Ibuprofen 400mg"], Stock = 80, Price = 15.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 2, MedicineId = medicineIds["Metformin 500mg"], Stock = 25, Price = 12.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 3, MedicineId = medicineIds["Aspirin 100mg"], Stock = 90, Price = 8.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 3, MedicineId = medicineIds["Vitamin D3 1000IU"], Stock = 45, Price = 36.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 3, MedicineId = medicineIds["Atorvastatin 20mg"], Stock = 20, Price = 41.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 4, MedicineId = medicineIds["Cetirizine 10mg"], Stock = 70, Price = 14.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 4, MedicineId = medicineIds["Omeprazole 20mg"], Stock = 55, Price = 22.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 5, MedicineId = medicineIds["Domperidone 10mg"], Stock = 35, Price = 18.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 5, MedicineId = medicineIds["Hydrocortisone Cream 1%"], Stock = 40, Price = 25.50m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 5, MedicineId = medicineIds["Clotrimazole Cream 1%"], Stock = 30, Price = 20.50m, IsAvailable = true },

    // الجدد
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Amoxicillin 250mg"], Stock = 40, Price = 19.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Doxycycline 100mg"], Stock = 25, Price = 36.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 2, MedicineId = medicineIds["Paracetamol 250mg Syrup"], Stock = 60, Price = 13.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 2, MedicineId = medicineIds["Diclofenac 50mg"], Stock = 45, Price = 21.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 3, MedicineId = medicineIds["Vitamin B12 1000mcg"], Stock = 30, Price = 29.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 3, MedicineId = medicineIds["Omega 3 1000mg"], Stock = 20, Price = 46.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 4, MedicineId = medicineIds["Iron 65mg"], Stock = 35, Price = 16.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 4, MedicineId = medicineIds["Sitagliptin 100mg"], Stock = 15, Price = 86.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 5, MedicineId = medicineIds["Aspirin 81mg (Cardiac)"], Stock = 50, Price = 11.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 5, MedicineId = medicineIds["Bisoprolol 5mg"], Stock = 40, Price = 33.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Desloratadine 5mg"], Stock = 55, Price = 26.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 2, MedicineId = medicineIds["Montelukast 10mg"], Stock = 30, Price = 41.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 3, MedicineId = medicineIds["Pantoprazole 40mg"], Stock = 45, Price = 29.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 4, MedicineId = medicineIds["Metoclopramide 10mg"], Stock = 60, Price = 9.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 5, MedicineId = medicineIds["Betamethasone Cream 0.1%"], Stock = 25, Price = 23.00m, IsAvailable = true },
    new PharmacyMedicine { PharmacyId = 1, MedicineId = medicineIds["Mupirocin Ointment 2%"], Stock = 20, Price = 36.00m, IsAvailable = true },
};

            var pharmacyMedicinesToAdd = allPharmacyMedicines
                .Where(pm => !existingPharmacyMedicines
                    .Any(e => e.PharmacyId == pm.PharmacyId && e.MedicineId == pm.MedicineId))
                .ToList();

            if (pharmacyMedicinesToAdd.Any())
            {
                await context.PharmacyMedicines.AddRangeAsync(pharmacyMedicinesToAdd);
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
            // ضيفه بعد الـ Users Seed وقبل آخر }
            if (!context.Users.Any(u => u.Email == "elezaby@pharmacy.com"))
            {
                var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<pharmacy.domin.Identity.ApplicationUser>();

                var pharmacyAdmins = new List<(string Email, string Name, int PharmacyId)>
    {
        ("elezaby@pharmacy.com", "El Ezaby Admin", 1),
        ("seif@pharmacy.com", "Seif Admin", 2),
        ("eldawaa@pharmacy.com", "El Dawaa Admin", 3),
        ("ramses@pharmacy.com", "Ramses Admin", 4),
        ("zamalek@pharmacy.com", "Zamalek Admin", 5),
    };

                foreach (var (email, name, pharmacyId) in pharmacyAdmins)
                {
                    var user = new pharmacy.domin.Identity.ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = name,
                        EmailConfirmed = true,
                        PharmacyId = pharmacyId,
                        NormalizedEmail = email.ToUpper(),
                        NormalizedUserName = email.ToUpper(),
                        SecurityStamp = Guid.NewGuid().ToString()
                    };
                    user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");
                    context.Users.Add(user);
                }
                await context.SaveChangesAsync();

                // ضيف الـ Role لكل واحد
                var pharmacyAdminRole = context.Roles.FirstOrDefault(r => r.Name == "PharmacyAdmin");
                if (pharmacyAdminRole != null)
                {
                    var emails = new[] { "elezaby@pharmacy.com", "seif@pharmacy.com", "eldawaa@pharmacy.com", "ramses@pharmacy.com", "zamalek@pharmacy.com" };
                    foreach (var email in emails)
                    {
                        var user = context.Users.FirstOrDefault(u => u.Email == email);
                        if (user != null && !context.UserRoles.Any(ur => ur.UserId == user.Id))
                        {
                            context.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>
                            {
                                UserId = user.Id,
                                RoleId = pharmacyAdminRole.Id
                            });
                        }
                    }
                    await context.SaveChangesAsync();
                }
            }

        }

    }
}