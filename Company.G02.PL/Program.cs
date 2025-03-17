using Company.G02.BLL.Interfaces;
using Company.G02.BLL.repositories;
using Company.G02.PL.Mapping;
using Company.G02.PL.Services;
using CompanyG02.DAL.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.G02.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>(); // Allow DI For DepartmentRepository  
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>(); // Allow DI For EmployeeRepository  

            builder.Services.AddDbContext<CompanyDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));


            // Life Time
            //builder.Service.AddScoped();      // Create Object Life Time Per Request - UnReachable Object
            //builder.Service.AddTransient();  // Create Object Life Time Per Operation
            //builder.Service.AddSingleton();  // Create Object Life Time Per App


            builder.Services.AddScoped<IScopedService, ScopedService>();             // Per Request
            builder.Services.AddTransient<ITransentService, TransentService>();     // Per Operation
            builder.Services.AddSingleton<ISingletonService, SingletonService>();   // Per App



            // Allow DI From CompanyDbContext  
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
