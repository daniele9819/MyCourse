using FluentValidation;
using Microsoft.AspNetCore.Http;
using MyCourse.Models.InputModels;

namespace MyCourse.Models.Validators
{
    public class CourseEditValidator : AbstractValidator<CourseEditInputModel>
    {
        public CourseEditValidator(IHttpContextAccessor context)
        {
            RuleFor(model => model.Id)
                .NotEmpty();

            RuleFor(model => model.Title)
                .NotEmpty().WithMessage("Il titolo è obbligatorio")
                .MinimumLength(10).WithMessage("Il titolo dev'essere di almeno {MinLength} caratteri")
                .MaximumLength(100).WithMessage("Il titolo dev'essere al massimo di {MaxLength} caratteri")
                .Matches(@"^[\w\s\.']+$").WithMessage("Titolo non valido")
                .Must(title => !title.Contains("MyCourse")).WithMessage("Il titolo non può contenere la parola 'MyCourse'")
                .Remote(url: "/Courses/IsTitleAvailable", additionalFields: "Id", errorText: "Il titolo già esiste");

            RuleFor(model => model.Description)
                .MinimumLength(10).WithMessage("La descrizione dev'essere di almeno {MinLength} caratteri")
                .MaximumLength(4000).WithMessage("La descrizione dev'essere al massimo {MaxLength} caratteri");

            RuleFor(model => model.Email)
                .EmailAddress().WithMessage("Devi inserire un indirizzo email");

            RuleFor(model => model.FullPrice)
                .NotEmpty().WithMessage("Il prezzo intero è obbligatorio");

            RuleFor(model => model.CurrentPrice)
                .NotEmpty().WithMessage("Il prezzo intero è obbligatorio")
                .Must((model, currentPrice) => currentPrice.Currency == model.FullPrice.Currency).WithMessage("Il prezzo corrente deve avere la stessa valuta del prezzo intero")
                .Must((model, currentPrice) => currentPrice.Amount <= model.FullPrice.Amount).WithMessage("Il prezzo corrente deve essere inferiore al prezzo intero");
                
        }
    }
}