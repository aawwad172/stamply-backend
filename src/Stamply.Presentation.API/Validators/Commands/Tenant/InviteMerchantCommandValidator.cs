using FluentValidation;

using Stamply.Application.CQRS.Commands.Tenant;
using Stamply.Domain.Enums;

namespace Stamply.Presentation.API.Validators.Commands.Tenant;

public class InviteMerchantCommandValidator : AbstractValidator<InviteMerchantCommand>
{
    public InviteMerchantCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("A valid email address is required");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required")
            .IsInEnum()
            .WithMessage("Valid role is required");
    }
}
