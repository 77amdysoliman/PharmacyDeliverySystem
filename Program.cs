using Microsoft.EntityFrameworkCore;
using pharmacy.Application.Interfaces;
using pharmacy.Application.Services;
using pharmacy.Application.Sevices;
using pharmacy.domin.Interfaces;
using pharmacy.infrastructuree.Data;
using pharmacy.infrastructuree.Repositories;

namespace Pharmacy
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration
            .GetConnectionString("PharmacyConnection"),
             b => b.MigrationsAssembly("pharmacy.infrastructuree")));
            // Add services to the container.


            // Dependency Injection  Repository + Unit of Work
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            //  Services
            builder.Services.AddScoped<IMedicineService, MedicineService>();
            builder.Services.AddScoped<IPharmacyService, PharmacyService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await AppDbContextSeed.SeedAsync(context);
            }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
