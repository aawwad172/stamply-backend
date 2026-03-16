using FluentValidation;

using Stamply.Application.CQRS.Commands.Tenant;

namespace Stamply.Presentation.API.Validators.Commands.Tenant;

public class SetupTenantCommandValidator : AbstractValidator<SetupTenantCommand>
{
    public SetupTenantCommandValidator()
    {
        RuleFor(x => x.BusinessEmail)
            .NotEmpty()
            .WithMessage("Business Email is required")
            .EmailAddress()
            .WithMessage("A valid Business Email is required");

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("Tenant Name is required");
    }
}
