using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastTechFoods.AuthService.Application.Commands.RegisterUser;
public class RegisterUserCommandValidator
        : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório")
            .Length(3, 100).WithMessage("O nome deve ter entre 3 e 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigatório")
            .EmailAddress().WithMessage("Formato de e-mail inválido");

        RuleFor(x => x.CPF)
            .Matches(@"^\d{11}$")
            .When(x => !string.IsNullOrEmpty(x.CPF))
            .WithMessage("CPF deve ter 11 dígitos numéricos");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .MinimumLength(6).WithMessage("A senha deve ter ao menos 6 caracteres");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("O papel é obrigatório");
    }
}