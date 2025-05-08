using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.Interfaces.Repositories;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class BookBorrowingRequestDetailRepository : IBookBorrowingRequestDetailRepository
{
    private readonly LibraryDbContext _context;

    public BookBorrowingRequestDetailRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookBorrowingRequestDetail>> GetByRequestIdAsync(Guid requestId)
    {
        return await _context.BookBorrowingRequestDetails
            .Include(d => d.Book)
            .Where(d => d.BookBorrowingRequestId == requestId)
            .ToListAsync();
    }
}