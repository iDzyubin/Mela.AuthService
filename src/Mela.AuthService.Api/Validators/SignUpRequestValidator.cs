using System.Text.RegularExpressions;
using FluentValidation;
using Mela.AuthService.Api.Requests;

namespace Mela.AuthService.Api.Validators;

/// <summary>
///     Валидатор модели регистрации
/// </summary>
public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    private const string RequiredMessage = "Поле '{PropertyName}' является обязательным к заполнению";
    private const string MatchMessage = "Поле '{PropertyName}' не удовлетворяет требованиям ввода";
    private const string PasswordRegEx = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
    
    public SignUpRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(RequiredMessage)
            .EmailAddress()
            .WithMessage(MatchMessage)
            .WithName("Адрес электронной почты");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(RequiredMessage)
            .Must(password => Regex.IsMatch(input: password, pattern: PasswordRegEx))
            .WithMessage(MatchMessage)
            .WithName("Пароль");
    }
}