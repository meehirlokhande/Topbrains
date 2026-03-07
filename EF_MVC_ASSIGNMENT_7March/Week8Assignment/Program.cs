using Microsoft.EntityFrameworkCore;
using Week8Assignment.Data;
using Week8Assignment.Models;

namespace Week8Assignment
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseInMemoryDatabase("LibraryDb"));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                if (!db.Books.Any())
                {
                    db.Books.AddRange(
                        new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 39.99m },
                        new Book { Title = "Design Patterns", Author = "GoF", Price = 54.99m },
                        new Book { Title = "Refactoring", Author = "Martin Fowler", Price = 44.99m });
                    db.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
