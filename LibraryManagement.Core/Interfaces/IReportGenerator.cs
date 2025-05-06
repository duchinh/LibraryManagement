using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces
{
    public interface IReportGenerator
    {
        Task<byte[]> GenerateBookReportAsync(string format);
        Task<byte[]> GenerateBorrowReportAsync(string format, string? startDate = null, string? endDate = null);
        Task<byte[]> GenerateOverdueReportAsync(string format);
        Task<byte[]> GenerateUserActivityReportAsync(string format, string? startDate = null, string? endDate = null);
        Task<byte[]> GenerateCategoryStatisticsReportAsync(string format);
        Task<byte[]> GeneratePopularBooksReportAsync(string format, int topCount = 10);
    }
} 