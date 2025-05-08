using System;
using System.Threading.Tasks;

namespace LibraryManagement.Core.Interfaces
{
    public interface IReportService
    {
        Task<byte[]> GenerateBookReportAsync(string format);
        Task<byte[]> GenerateBorrowReportAsync(string format, DateTime startDate, DateTime endDate);
        Task<byte[]> GenerateOverdueReportAsync(string format);
        Task<byte[]> GenerateUserActivityReportAsync(string format, DateTime startDate, DateTime endDate);
        Task<byte[]> GenerateCategoryStatisticsReportAsync(string format);
        Task<byte[]> GeneratePopularBooksReportAsync(string format, int topCount);
    }
}