using FluentValidation;

namespace HD.Application.Tickets.Commands.UpdateTicket;

public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Ticket ID is required");

        When(x => x.Dto.Title != null, () =>
        {
            RuleFor(x => x.Dto.Title)
                .NotEmpty().WithMessage("Title cannot be empty")
                .MaximumLength(500).WithMessage("Title must not exceed 500 characters");
        });

        When(x => x.Dto.Description != null, () =>
        {
            RuleFor(x => x.Dto.Description)
                .NotEmpty().WithMessage("Description cannot be empty")
                .MaximumLength(5000).WithMessage("Description must not exceed 5000 characters");
        });

        When(x => x.Dto.Category != null, () =>
        {
            RuleFor(x => x.Dto.Category)
                .NotEmpty().WithMessage("Category cannot be empty")
                .MaximumLength(100).WithMessage("Category must not exceed 100 characters");
        });

        When(x => x.Dto.Status.HasValue, () =>
        {
            RuleFor(x => x.Dto.Status)
                .IsInEnum().WithMessage("Invalid status value");
        });

        When(x => x.Dto.Priority.HasValue, () =>
        {
            RuleFor(x => x.Dto.Priority)
                .IsInEnum().WithMessage("Invalid priority value");
        });
    }
}
