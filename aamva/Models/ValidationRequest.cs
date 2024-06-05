using System.ComponentModel.DataAnnotations;
using static Aamva.Utils.SoapUtils;

namespace Aamva.Models;

public class ValidationRequest : IValidatableObject
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? Suffix { get; set; }

    public string? Dob { get; set; }
    public string? Gender { get; set; }

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }

    [Required]
    public string IdNumber { get; set; }
    public string? IssueDate { get; set; }
    public string? ExpirationDate { get; set; }

    public string? IdType { get; set; }

    public string? Weight { get; set; }
    public string? EyeColor { get; set; }
    public string? Height { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!CheckDate(Dob))
        {
            yield return new ValidationResult($"Specified {nameof(Dob)} is not a valid date.", new[] { nameof(Dob) });
        }
        if (!CheckDate(IssueDate))
        {
            yield return new ValidationResult($"Specified {nameof(IssueDate)} is not a valid date.", new[] { nameof(IssueDate) });
        }
        if (!CheckDate(ExpirationDate))
        {
            yield return new ValidationResult($"Specified {nameof(ExpirationDate)} is not a valid date.", new[] { nameof(ExpirationDate) });
        }
        if (!CheckSex(Gender))
        {
            yield return new ValidationResult($"Specified {nameof(Gender)} is not a valid sex.", new[] { nameof(Gender) });
        }
        if (!CheckState(State))
        {
            yield return new ValidationResult($"Specified {nameof(State)} is not a valid state.", new[] { nameof(State) });
        }
        if (!CheckDocCat(IdType))
        {
            yield return new ValidationResult($"Specified {nameof(IdType)} is not a valid document category.", new[] { nameof(IdType) });
        }
        if (!CheckEyeColor(EyeColor))
        {
            yield return new ValidationResult($"Specified {nameof(EyeColor)} is not a valid eye color.", new[] { nameof(EyeColor) });
        }
    }
}

