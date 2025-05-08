using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;
namespace LibraryManagement.Core.Interfaces.Services
{
    public interface IBorrowRequestService
    {
        Task<BorrowingRequestDTO> BorrowBooksAsync(CreateBorrowingRequestDTO dto);
        Task<List<BorrowingRequestDTO>> GetAllRequestsForUserAsync(Guid userId);
        Task<List<BorrowingRequestDTO>> GetAllRequestsAsync();
        Task<BorrowingRequestDTO> UpdateRequestStatusAsync(Guid requestId, UpdateBorrowingRequestDTO dto);
    }
}