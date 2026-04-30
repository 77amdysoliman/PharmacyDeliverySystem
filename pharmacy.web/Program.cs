using Microsoft.EntityFrameworkCore;
using pharmacy.Application.Interfaces;
using pharmacy.Application.Services;
using pharmacy.Application.Sevices;
using pharmacy.domin.Interfaces;
using pharmacy.infrastructuree.Data;
using pharmacy.infrastructuree.Repositories; 

namespace pharmacy.web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // ── Repositories & UnitOfWork ─────────────────────────
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ── Application Services ──────────────────────────────
            builder.Services.AddScoped<IMedicineService, MedicineService>();
            builder.Services.AddScoped<IPharmacyService, PharmacyService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            // Add services to the container.
            builder.Services.AddRazorPages();


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                await AppDbContextSeed.SeedAsync(db);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}