using FluentValidation;
using FluentValidation.Results;
using ValidationException = Application.Common.Exceptions.ValidationException;

namespace Application.Extensions
{
    public static class ValidationExtension
    {
        public static async Task<ValidationResult> ValidateAsync<T>(this T request, IValidator<T> validator, CancellationToken cancellationToken = default)
        {

            ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);
     
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return validationResult;
        }
    }
}