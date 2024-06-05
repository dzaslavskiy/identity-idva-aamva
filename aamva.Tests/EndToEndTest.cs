using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aamva.Tests.Utils;
using Aamva.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Aamva.Tests;

[TestCaseOrderer("Aamva.Tests.Utils." + nameof(AlphabeticalOrderer), "Aamva.Tests")]
public class EndToEndTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public EndToEndTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        _factory = factory.WithWebHostBuilder(
            builder => builder.ConfigureLogging(
                loggingbuilder => loggingbuilder.Services.AddSingleton<ILoggerProvider>(
                    serviceProvider => new Microsoft.Extensions.Logging.Testing.XunitLoggerProvider(_output))));
    }

    [Fact]
    public async Task Pennsylvania01()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""55039350"",

            ""firstName"": ""AAMVANCDL"",
            ""lastName"": ""PENNDOT"",
            ""middleName"": ""ONE"",
            ""suffix"": ""SR"",
            
            ""dob"": ""9/10/1990"",

            ""gender"": ""male"",
            ""idType"": ""driver license"",

            ""addressLine1"" : ""1101 S FRONT ST"",
            ""city"": ""HARRISBURG"",
            ""state"": ""PA"",
            ""zip"": ""17104"",

            ""expirationDate"": ""9/11/2025"",
            ""issueDate"": ""9/10/2021""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

        [Fact]
    public async Task Pennsylvania02()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""55039351"",

            ""firstName"": ""AAMVANCDL"",
            ""lastName"": ""PENNDOT"",
            ""middleName"": ""TWO"",
            
            ""dob"": ""9/10/1988"",

            ""gender"": ""female"",
            ""idType"": ""driver license"",

            ""addressLine1"" : ""1101 S FRONT ST"",
            ""city"": ""HARRISBURG"",
            ""state"": ""PA"",
            ""zip"": ""17104"",

            ""expirationDate"": ""9/11/2025"",
            ""issueDate"": ""9/10/2021""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task Pennsylvania03()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""55039353"",

            ""firstName"": ""AAMVANCDL"",
            ""lastName"": ""PENNDOT"",
            ""middleName"": ""THREE"",
            
            ""dob"": ""9/10/1980"",

            ""idType"": ""driver permit"",

            ""addressLine1"" : ""1101 S FRONT ST"",
            ""city"": ""HARRISBURG"",
            ""state"": ""PA"",
            ""zip"": ""17104"",

            ""expirationDate"": ""9/11/2025"",
            ""issueDate"": ""9/10/2021""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task Oregon01()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""C048843"",

            ""firstName"": ""HYPHENFIRST"",
            ""lastName"": ""DLDV"",
            ""middleName"": ""MIDDLE SPACE"",
            
            ""dob"": ""1993-02-24"",

            ""eyeColor"": ""Blue"",
            ""gender"": ""male"",

            ""addressLine1"" : ""1 W C ST"",
            ""city"": ""BURNS"",
            ""state"": ""OR"",
            ""zip"": ""977201200"",

            ""height"": ""505"",
            ""weight"": ""214"",

            ""expirationDate"": ""2028-02-24"",
            ""issueDate"": ""2020-06-13""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task Oregon02()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""C048842"",

            ""firstName"": ""DLDV"",
            ""lastName"": ""LONGMULTIPLEADDRESSES"",
            ""middleName"": ""MIDDLE"",
            
            ""dob"": ""1992-12-25"",

            ""eyeColor"": ""pink"",
            ""gender"": ""male"",

            ""addressLine1"" : ""RLYLONGMALADDRESSLN1"",
            ""city"": ""REALLYLONGCITYY"",
            ""state"": ""OR"",
            ""zip"": ""123456789"",

            ""height"": ""509"",
            ""weight"": ""151"",

            ""expirationDate"": ""2028-12-25"",
            ""issueDate"": ""2020-06-13""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }
}