using FluentValidation;
using LibraryManagement.Core.DTOs;

namespace LibraryManagement.Application.Validators
{
    public class CreateBorrowingRequestDTOValidator : AbstractValidator<CreateBorrowingRequestDTO>
    {
        public CreateBorrowingRequestDTOValidator()
        {
            RuleFor(x => x.BookIds)
                .NotEmpty().WithMessage("Danh sách sách không được để trống")
                .Must(ids => ids.Count <= 5).WithMessage("Không được mượn quá 5 cuốn sách cùng lúc")
                .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Không được chọn trùng sách");
        }
    }
}