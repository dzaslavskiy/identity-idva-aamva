using System;
using System.Collections.Generic;
using Aamva.Models;
using Aamva.Services;
using Xunit;

namespace Aamva.Tests;

public class DldvServiceTest
{
    [Fact]
    public void ToSoapRequest_Valid_ReturnValid()
    {
        var s = new ValidationRequest
        {
            FirstName = "John",
            LastName = "Smith",
            MiddleName = "(joe)",
            Suffix = "Jr.",

            Dob = "1902-01-01",
            Gender = "male",

            AddressLine1 = "1002 Main st.",
            AddressLine2 = "Apt #202",
            City = "Carralton",
            State = "Va",
            Zip = "20202",

            IdNumber = "8923498273432",
            IssueDate = "2020-01-01",
            ExpirationDate = "2023-01-01",

            IdType = "driver license",

            Weight = "100",
            EyeColor = "green",
            Height = "510",
        };

        var r = DldvService.ToSoapRequest(s, "", "", "");

        Assert.True(r.DriverLicenseIdentification.IdentificationID.Value == s.IdNumber);
    }

    [Fact]
    public void ToSoapRequest_InvalidSex_ReturnException()
    {
        var s = new ValidationRequest
        {
            Gender = "zmale",
            IdNumber = "8923498273432",
        };

        Assert.Throws<KeyNotFoundException>(() => DldvService.ToSoapRequest(s, "", "", ""));
    }

    [Fact]
    public void ToSoapRequest_InvalidDate_ThrowsException()
    {
        var s = new ValidationRequest
        {
            Dob = "f1902-01-01",
            IdNumber = "8923498273432"
        };

        Assert.Throws<FormatException>(() => DldvService.ToSoapRequest(s, "", "", ""));
    }

    [Fact]
    public void ToSoapRequest_InvalidState_ThrowsException()
    {
        var s = new ValidationRequest
        {
            State = "ZZ",
            IdNumber = "8923498273432"
        };

        Assert.Throws<ArgumentException>(() => DldvService.ToSoapRequest(s, "", "", ""));
    }

    [Fact]
    public void ToSoapRequest_InvalidDocCat_ThrowsException()
    {
        var s = new ValidationRequest
        {
            IdNumber = "8923498273432",
            IdType = "dl",
        };

        Assert.Throws<KeyNotFoundException>(() => DldvService.ToSoapRequest(s, "", "", ""));
    }

    [Fact]
    public void ToSoapRequest_InvalidEyeColor_ThrowsException()
    {
        var s = new ValidationRequest
        {
            IdNumber = "8923498273432",
            EyeColor = "agreen",
        };

        Assert.Throws<KeyNotFoundException>(() => DldvService.ToSoapRequest(s, "", "", ""));
    }

    [Theory]
    [InlineData("green")]
    [InlineData("Green")]
    [InlineData("GREEN")]
    public void ToSoapRequest_ValidEyeColor_Valid(string color)
    {
        var s = new ValidationRequest
        {
            IdNumber = "8923498273432",
            EyeColor = color,
        };

        var r = DldvService.ToSoapRequest(s, "", "", "");
        Assert.True(r.PersonEyeColorCode.Value == DldvServiceReference.PersonEyeColorCodeSimpleType.GRN);
    }
}