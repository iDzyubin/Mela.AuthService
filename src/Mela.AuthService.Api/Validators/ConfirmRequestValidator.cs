using System.Text.RegularExpressions;
using FluentValidation;
using Mela.AuthService.Api.Requests;

namespace Mela.AuthService.Api.Validators;

public class ConfirmRequestValidator : AbstractValidator<ConfirmRequest>
{
    private const string RequiredMessage = "Поле '{PropertyName}' является обязательным к заполнению";
    private const string MatchMessage = "Поле '{PropertyName}' не удовлетворяет требованиям ввода";
    
    public ConfirmRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(RequiredMessage)
            .EmailAddress()
            .WithMessage(MatchMessage)
            .WithName("Адрес электронной почты");

        RuleFor(x => x.ConfirmCode)
            .NotEmpty()
            .WithMessage(RequiredMessage)
            .Must(code => Regex.IsMatch(input: code, pattern: @"\d{6}"))
            .WithMessage(MatchMessage)
            .WithName("Код подтверждения");
    }
}