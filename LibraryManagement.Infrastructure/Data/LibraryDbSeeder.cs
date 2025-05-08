using System.Security.Cryptography;
using System.Text;
using Bogus;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.Data
{
    public class LibraryDbSeeder
    {
        public static async Task SeedCategoriesAsync(LibraryDbContext context, ILogger<LibraryDbSeeder> logger)
        {
            if (!context.Categories.Any())
            {
                var categoryFaker = new Faker<Category>()
                    .RuleFor(c => c.Id, f => Guid.NewGuid())
                    .RuleFor(c => c.Name, f => f.PickRandom(new[]
                    {
                        "Tiểu thuyết",
                        "Phi hư cấu",
                        "Khoa học viễn tưởng",
                        "Trinh thám",
                        "Lãng mạn",
                        "Tiểu sử",
                        "Lịch sử",
                        "Khoa học",
                        "Công nghệ",
                        "Kinh doanh",
                        "Tự lực",
                        "Thơ ca",
                        "Kịch",
                        "Kinh dị",
                        "Phiêu lưu",
                        "Giả tưởng",
                        "Văn học thiếu nhi",
                        "Thanh niên",
                        "Truyện tranh",
                        "Nghệ thuật & Nhiếp ảnh"
                    }))
                    .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
                    .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow);

                var categories = categoryFaker.Generate(20)
                    .GroupBy(c => c.Name)
                    .Select(g => g.First())
                    .ToList();

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();

                logger.LogInformation("Đã tạo và seed {Count} danh mục sách", categories.Count);
            }
            else
            {
                logger.LogInformation("Danh mục sách đã tồn tại. Không cần seed.");
            }
        }

        public static async Task SeedBooksAsync(LibraryDbContext context, ILogger<LibraryDbSeeder> logger)
        {
            await SeedCategoriesAsync(context, logger);

            if (!context.Books.Any())
            {
                var categoryIds = await context.Categories.Select(c => c.Id).ToListAsync();

                var bookFaker = new Faker<Book>()
                    .RuleFor(b => b.Id, f => Guid.NewGuid())
                    .RuleFor(b => b.Title, f => f.Commerce.ProductName())
                    .RuleFor(b => b.Author, f => f.Name.FullName())
                    .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
                    .RuleFor(b => b.PublicationYear, f => f.Random.Int(1900, 2024))
                    .RuleFor(b => b.Quantity, f => f.Random.Int(5, 20))
                    .RuleFor(b => b.AvailableQuantity, (f, b) => f.Random.Int(1, b.Quantity))
                    .RuleFor(b => b.CreatedAt, f => DateTime.UtcNow)
                    .RuleFor(b => b.CategoryId, f => f.PickRandom(categoryIds));

                var books = bookFaker.Generate(60);

                await context.Books.AddRangeAsync(books);
                await context.SaveChangesAsync();

                logger.LogInformation("Đã tạo và seed {Count} sách", books.Count);
            }
            else
            {
                logger.LogInformation("Sách đã tồn tại. Không cần seed.");
            }
        }

        public static async Task SeedAdminUserAsync(LibraryDbContext context, ILogger<LibraryDbSeeder> logger)
        {
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var adminPassword = "Admin@123";
                var hashedPassword = HashPassword(adminPassword);

                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "System",
                    UserName = "admin",
                    Email = "admin@library.com",
                    PhoneNumber = "0123456789",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false,
                    IsEmailVerified = true
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

                logger.LogInformation("Đã tạo tài khoản admin. Username: {UserName}, Email: {Email}",
                    adminUser.UserName,
                    adminUser.Email);
            }
            else
            {
                logger.LogInformation("Tài khoản admin đã tồn tại. Không cần seed.");
            }
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
