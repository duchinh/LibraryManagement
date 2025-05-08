using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

// Add DbContext
services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer("Server=DESKTOP-R5D72CE;Database=LibraryManagement;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"));

// Add logging
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

var serviceProvider = services.BuildServiceProvider();

// Seed data
using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<LibraryDbSeeder>>();

    try
    {
        // Đảm bảo database được tạo
        context.Database.EnsureCreated();

        // Seed data
        await LibraryDbSeeder.SeedCategoriesAsync(context, logger);
        await LibraryDbSeeder.SeedBooksAsync(context, logger);
        await LibraryDbSeeder.SeedAdminUserAsync(context, logger);

        logger.LogInformation("Đã seed dữ liệu thành công");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Có lỗi xảy ra khi seed dữ liệu");
    }
}