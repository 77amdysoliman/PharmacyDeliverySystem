using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pharmacy.Application.Interfaces;
using pharmacy.Application.Services;
using pharmacy.Application.Sevices;
using pharmacy.domin.Identity;
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
            builder.Services.AddScoped<IEmailService, EmailService>();
            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(builder.Configuration
        .GetConnectionString("PharmacyConnection"),
        b => b.MigrationsAssembly("pharmacy.infrastructuree")));

            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            //  Cookie Settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddScoped<IMedicineService, MedicineService>();
            builder.Services.AddScoped<IPharmacyService, PharmacyService>();
            builder.Services.AddScoped<IOrderService, OrderService>();


            builder.Services.AddRazorPages();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var app = builder.Build();

            //  app.MapGet("/", context => {
            //    context.Response.Redirect("/Dashboard");
            //  return Task.CompletedTask;
            //});

            // Seed Roles + Super Admin


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<AppDbContext>();
                await AppDbContextSeed.SeedAsync(context);

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                await SeedRolesAsync(roleManager, userManager);
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

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();


            //  Seed Roles Function
            static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
            {
                //   Roles
                string[] roles = { "SuperAdmin", "PharmacyAdmin", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                var pharmacyAdmin = await userManager.FindByEmailAsync("Pharmacyadmin2@gmail.com");
                if (pharmacyAdmin != null && !await userManager.IsInRoleAsync(pharmacyAdmin, "PharmacyAdmin"))
                {
                    await userManager.AddToRoleAsync(pharmacyAdmin, "PharmacyAdmin");
                }

                //  Super Admin
                var superAdminEmail = "superadmin@pharmacy.com";
                var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
                if (superAdmin == null)
                {
                    superAdmin = new ApplicationUser
                    {
                        UserName = superAdminEmail,
                        Email = superAdminEmail,
                        FullName = "Super Admin",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(superAdmin, "Admin@123");
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }
            }
        }
    }
}