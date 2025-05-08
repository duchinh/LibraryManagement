using AutoMapper;
using LibraryManagement.Core.Entities;
using LibraryManagement.Core.DTOs;
using LibraryManagement.Core.Enums;
namespace LibraryManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTOs>();

            CreateMap<UserDTOs, User>();

            CreateMap<UpdateUserDto, User>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Book, BookDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CreateBookDTO, Book>();

            CreateMap<UpdateBookDTO, Book>();

            CreateMap<Category, CategoryDto>();

            CreateMap<CategoryDto, Category>();

            CreateMap<CreateCategoryDTO, Category>();

            CreateMap<UpdateCategoryDTO, Category>();

            CreateMap<BorrowingRequestDTO, BookBorrowingRequest>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => RequestStatus.Waiting))
            .ForMember(dest => dest.BookBorrowingRequestDetails, opt => opt.Ignore());

            CreateMap<BookBorrowingRequest, BorrowingRequestDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.BookTitles, opt => opt.MapFrom(src =>
                    src.BookBorrowingRequestDetails.Where(b => b.Book != null)
                    .Select(b => b.Book.Title).ToList()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<UpdateBorrowingRequestDTO, BookBorrowingRequest>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<BookBorrowingRequestDetail, BorrowingRequestDetailDTO>();
        }
    }
}
