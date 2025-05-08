using LibraryManagement.Core.Entities;

namespace LibraryManagement.Core.Interfaces.Repositories
{
    public interface IBookBorrowingRequestDetailRepository
    {
        Task<List<BookBorrowingRequestDetail>> GetByRequestIdAsync(Guid requestId);
    }

}