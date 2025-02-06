using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred")
    {
        Errors = new Dictionary<string, string[]>();
    }
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {   
        Errors = failures.GroupBy(f => f.PropertyName)
                       .ToDictionary(g => g.Key, g => g.Select( e => $"\n{e.ErrorMessage}").ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }

    public string GetErrors()
    {
        string errors = string.Empty;
        foreach ((string propertyName, string[] errorMessages) in Errors)
        {
            errors += ($" {string.Join("\n ", errorMessages)}");//used to be comma
        }
        return errors;
    }
}