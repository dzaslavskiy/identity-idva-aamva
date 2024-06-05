namespace Aamva.Models;

public class MatchResponse
{
    public string? LocatorId { get; set; }

    public bool? DriverLicenseNumber { get; set; }
    public bool? DriverLicenseIssueDate { get; set; }
    public bool? DriverLicenseExpirationDate { get; set; }

    public bool? DocumentCategory { get; set; }

    public bool? PersonLastNameExact { get; set; }
    public bool? PersonLastNameFuzzyPrimary { get; set; }
    public bool? PersonLastNameFuzzyAlternate { get; set; }
    public bool? PersonFirstNameExact { get; set; }
    public bool? PersonFirstNameFuzzyPrimary { get; set; }
    public bool? PersonFirstNameFuzzyAlternate { get; set; }
    public bool? PersonMiddleNameExact { get; set; }
    public bool? PersonMiddleNameFuzzyPrimary { get; set; }
    public bool? PersonMiddleNameFuzzyAlternate { get; set; }
    public bool? PersonMiddleInitial { get; set; }
    public bool? PersonNameSuffix { get; set; }

    public bool? PersonBirthDate { get; set; }
    public bool? PersonSexCode { get; set; }

    public bool? PersonHeight { get; set; }
    public bool? PersonWeight { get; set; }
    public bool? PersonEyeColor { get; set; }

    public bool? AddressLine1 { get; set; }
    public bool? AddressLine2 { get; set; }
    public bool? AddressCity { get; set; }
    public bool? AddressStateCode { get; set; }
    public bool? AddressZIP5 { get; set; }
    public bool? AddressZIP4 { get; set; }
}