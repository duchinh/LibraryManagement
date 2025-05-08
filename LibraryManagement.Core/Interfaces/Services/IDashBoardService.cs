using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatisticsDto> GetDashboardAsync();
    }
}