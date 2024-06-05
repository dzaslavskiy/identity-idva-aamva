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
public class StructuredTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public StructuredTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        _factory = factory.WithWebHostBuilder(
            builder => builder
                .ConfigureLogging(
                    loggingbuilder => loggingbuilder.Services.AddSingleton<ILoggerProvider>(
                        serviceProvider => new Microsoft.Extensions.Logging.Testing.XunitLoggerProvider(_output)))
                .ConfigureAppConfiguration((hostingContext, config) => config.Build()["UseAAMVATestSimulator"] = "true"));
    }

    [Fact]
    public async Task TestCase01()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST11"",
            ""firstName"": ""STEVEN"",
            ""lastName"": ""BROWN"",
            ""dob"": ""1980-01-01""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase02()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST12"",
            ""firstName"": ""STEVEN"",
            ""lastName"": ""BROWN"",
            ""middleName"": ""DANIEL"",
            ""suffix"": ""JR"",
            ""dob"": ""1980-01-01"",
            ""eyeColor"": ""Brown"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4301 Wilson Blvd"",
            ""addressLine2"": ""1234"",
            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",

            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-10-10"",
            ""issueDate"": ""2011-10-10""
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

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase03()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST22"",
            ""firstName"": ""BROWN"",
            ""lastName"": ""KATTY"",

            ""dob"": ""1982-12-23"",
            ""eyeColor"": ""Brown"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""123 GREEN RD"",
            ""addressLine2"": ""123"",
            ""city"": ""ASHBURN"",
            ""state"": ""VA"",
            ""zip"": ""20148"",

            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-11-11"",
            ""issueDate"": ""2011-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase04()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVLONGESTNAME123"",
            ""firstName"": ""FIRSTNAME"",
            ""lastName"": ""LASTNAMEISLONGERXXXXXXXXX"",

            ""dob"": ""1980-02-29""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase05()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVLONGESTNAME456"",
            ""firstName"": ""FIRSTNAMELONG"",
            ""lastName"": ""LASTNAMELONGE"",
            ""middleName"": ""MLONG"",

            ""dob"": ""1980-02-29"",

            ""eyeColor"": ""Brown"",
            ""gender"": ""male"",
            ""idType"": ""driver permit"",

            ""addressLine1"" : ""1 WATER BLVD"",
            ""addressLine2"": ""STE 103"",
            ""city"": ""ARLINGTON"",
            ""state"": ""VA"",
            ""zip"": ""22203"",

            ""height"": ""600"",
            ""weight"": ""300"",
            ""expirationDate"": ""2013-10-10"",
            ""issueDate"": ""2005-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.False(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.False(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.False(matches.GetProperty("personSexCode").GetBoolean());
        Assert.False(matches.GetProperty("personHeight").GetBoolean());
        Assert.False(matches.GetProperty("personWeight").GetBoolean());

        Assert.False(matches.GetProperty("documentCategory").GetBoolean());

        Assert.False(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase06()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""PERMITCARD123"",

            ""firstName"": ""SMITH"",
            ""lastName"": ""JOHNSON"",
            ""suffix"": ""JR"",

            ""dob"": ""1980-02-29""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase07()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""PERMITCARD456"",
            ""firstName"": ""JOHNSON"",
            ""lastName"": ""SMITH"",
            ""suffix"": ""SR"",

            ""dob"": ""1980-01-05"",
            ""gender"": ""male"",
            ""idType"": ""Driver Permit"",

            ""addressLine1"" : ""5467 Whiele Ave"",
            ""city"": ""Reston"",
            ""state"": ""VA"",
            ""zip"": ""20123"",

            ""expirationDate"": ""2020-04-04"",
            ""issueDate"": ""2012-12-12""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());

        Assert.False(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.False(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personSexCode").GetBoolean());

        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.False(matches.GetProperty("addressLine1").GetBoolean());

        Assert.False(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.False(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.False(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase08()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""STATECARD123"",
            ""firstName"": ""JACKSON"",
            ""lastName"": ""JACKSON"",

            ""dob"": ""1970-12-31"",
            ""eyeColor"": ""Brown"",
            ""gender"": ""F"",
            ""idType"": ""id card"",
            ""addressLine1"" : ""4301 Wilson Blvd"",
            ""addressLine2"": ""1234"",
            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",

            ""height"": ""510"",
            ""weight"": ""250"",
            ""expirationDate"": ""2020-10-10"",
            ""issueDate"": ""2010-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.False(matches.GetProperty("addressLine1").GetBoolean());

        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.False(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase09()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""STATECARD456"",
            ""firstName"": ""MICHAEL"",
            ""lastName"": ""JACKSON"",
            ""middleName"": ""A"",
            ""suffix"": ""Jr"",

            ""dob"": ""1970-10-09"",
            ""eyeColor"": ""black"",
            ""gender"": ""female"",
            ""idType"": ""id card"",
            ""addressLine1"" : ""2301 TWEAK RD"",
            ""addressLine2"": ""4444"",
            ""city"": ""VIENNA"",
            ""state"": ""VA"",
            ""zip"": ""11111"",

            ""height"": ""510"",
            ""weight"": ""250"",
            ""expirationDate"": ""2015-01-01"",
            ""issueDate"": ""2011-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.False(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.False(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.False(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.False(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase10()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""NOMATCH123"",
            ""firstName"": ""THEREISNOMATCH"",
            ""lastName"": ""FORTHEDLDV"",
            ""middleName"": ""REQUEST"",

            ""dob"": ""1956-05-31"",
            ""eyeColor"": ""Brown"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4301 Wilson Blvd"",

            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",

            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2021-02-28""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.False(matches.GetProperty("driverLicenseNumber").GetBoolean());
    }

    [Fact]
    public async Task TestCase11()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"":  ""REPORTEDDECEASED1234"",
            ""firstName"": ""DECEASED"",
            ""lastName"": ""PERSON"",
            ""middleName"": ""IS"",
            ""dob"":  ""1956-05-31"",
            ""eyeColor"":  ""Brown"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""2320 ROLEX ST"",
            ""city"": ""FAIRFAX"",
            ""state"": ""VA"",
            ""zip"": ""12345"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2016-02-29""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.False(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase12()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""INCOMPLETEDATA123"",
            ""firstName"": ""NATIVE"",
            ""lastName"": ""CHICAGO"",
            ""middleName"": ""JERRY"",
            ""dob"": ""1979-01-01"",
            ""eyeColor"": ""maroon"",
            ""gender"": ""female"",
            ""idType"": ""driver permit"",
            ""addressLine1"" : ""4301 WILSON BLVD"",
            ""city"": ""ARLINGTON"",
            ""state"": ""VA"",
            ""zip"": ""22203"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2016-02-29""
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

        Assert.False(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.False(matches.GetProperty("personSexCode").GetBoolean());
        Assert.False(matches.GetProperty("documentCategory").GetBoolean());

        Assert.False(matches.GetProperty("addressLine1").GetBoolean());
        Assert.False(matches.GetProperty("addressCity").GetBoolean());
        Assert.False(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.False(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.False(matches.GetProperty("personHeight").GetBoolean());
        Assert.False(matches.GetProperty("personWeight").GetBoolean());

        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase13()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVFUZZYLOGICTEST11"",
            ""firstName"": ""STEVEN"",
            ""lastName"": ""BROWN"",
            ""middleName"": ""SCOTT"",
            ""dob"": ""1980-01-01""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.False(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personMiddleNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase14()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST13"",
            ""firstName"": ""ELLI"",
            ""lastName"": ""ROGER"",
            ""middleName"": ""KING"",
            ""suffix"": ""JR"",
            ""dob"": ""1970-02-03"",
            ""eyeColor"": ""GRAY"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4301 Wilson Blvd"",
            ""addressLine2"": ""1234"",
            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2021-01-01"",
            ""issueDate"": ""2013-01-01""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.False(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personMiddleNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.False(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.False(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase15()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST14"",
            ""firstName"": ""RUBY"",
            ""lastName"": ""HASAN"",
            ""dob"": ""1970-01-01"",
            ""eyeColor"": ""BROWN"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4444 ASTORIA CIR"",
            ""addressLine2"": ""STE 102"",
            ""city"": ""CENTERVILLE"",
            ""state"": ""VA"",
            ""zip"": ""45678"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2013-10-10"",
            ""issueDate"": ""2011-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.False(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.False(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.False(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase16()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST12"",
            ""firstName"": ""STEPHEN"",
            ""lastName"": ""BROWN"",
            ""middleName"": ""DANIEL"",
            ""suffix"": ""JR"",
            ""dob"": ""1980-01-01"",
            ""eyeColor"": ""BROWN"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4301 Wilson Blvd"",
            ""addressLine2"": ""1234"",
            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-10-10"",
            ""issueDate"": ""2011-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personFirstNameExact").GetBoolean());

        Assert.True(matches.GetProperty("personFirstNameFuzzyPrimary").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameFuzzyAlternate").GetBoolean());

        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase17()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST12"",
            ""firstName"": ""STEVEN"",
            ""lastName"": ""BROWN"",
            ""middleName"": ""DANEEL"",
            ""suffix"": ""JR"",
            ""dob"": ""1980-01-01"",
            ""eyeColor"": ""BROWN"",
            ""gender"": ""MALE"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4301 Wilson Blvd"",
            ""addressLine2"": ""1234"",
            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-10-10"",
            ""issueDate"": ""2011-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.False(matches.GetProperty("personMiddleNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.True(matches.GetProperty("personMiddleNameFuzzyPrimary").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameFuzzyAlternate").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase18()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST12"",
            ""firstName"": ""STEVEN"",
            ""lastName"": ""BARON"",
            ""middleName"": ""DANIEL"",
            ""suffix"": ""JR"",
            ""dob"": ""1980-01-01"",
            ""eyeColor"": ""BROWN"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4301 Wilson Blvd"",
            ""addressLine2"": ""1234"",
            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-10-10"",
            ""issueDate"": ""2011-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.False(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personMiddleNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personNameSuffix").GetBoolean());

        Assert.True(matches.GetProperty("personLastNameFuzzyPrimary").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameFuzzyAlternate").GetBoolean());

        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase19A()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST16"",
            ""firstName"": ""JEFF"",
            ""lastName"": ""JONES"",
            ""dob"": ""1970-01-01"",
            ""eyeColor"": ""BROWN"",
            ""gender"": ""MALE"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""123-129 RES AD BLVD"",
            ""addressLine2"": ""RESI APT 1234A & 1234B"",
            ""city"": ""CITY OF RES AB"",
            ""state"": ""VA"",
            ""zip"": ""12345-1234"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-01-01"",
            ""issueDate"": ""2013-01-01""
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

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.True(matches.GetProperty("addressLine1").GetBoolean());
        Assert.True(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.True(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.True(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase19B()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST16"",
            ""firstName"": ""JEFF"",
            ""lastName"": ""JONES"",
            ""dob"": ""1970-01-01"",
            ""eyeColor"": ""BROWN"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""789-987 RES AD BLVD"",
            ""addressLine2"": ""RESI APT 6789Z & 6789Z"",
            ""city"": ""CITY OF RES AB"",
            ""state"": ""MD"",
            ""zip"": ""56789-6789"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-01-01"",
            ""issueDate"": ""2013-01-01""
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

        Assert.True(matches.GetProperty("personEyeColor").GetBoolean());
        Assert.True(matches.GetProperty("personSexCode").GetBoolean());
        Assert.True(matches.GetProperty("documentCategory").GetBoolean());

        Assert.False(matches.GetProperty("addressLine1").GetBoolean());
        Assert.False(matches.GetProperty("addressLine2").GetBoolean());
        Assert.True(matches.GetProperty("addressCity").GetBoolean());
        Assert.False(matches.GetProperty("addressStateCode").GetBoolean());
        Assert.False(matches.GetProperty("addressZIP5").GetBoolean());

        Assert.True(matches.GetProperty("personHeight").GetBoolean());
        Assert.True(matches.GetProperty("personWeight").GetBoolean());

        Assert.True(matches.GetProperty("driverLicenseIssueDate").GetBoolean());
        Assert.True(matches.GetProperty("driverLicenseExpirationDate").GetBoolean());
    }

    [Fact]
    public async Task TestCase20()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""eyeColor"": ""GRAY"",
            ""gender"": ""Female"",
            ""idType"": ""driver permit"",
            ""addressLine1"" : ""4301 WILSON BLVD"",
            ""city"": ""ARLINGTON"",
            ""state"": ""VA"",
            ""zip"": ""22203"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2016-02-29""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        Assert.False(response.IsSuccessStatusCode);

        var errors = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(errors.ToStringPretty());

        Assert.Equal("One or more validation errors occurred.", errors.GetProperty("title").GetString());
        Assert.Equal("One or more validation errors occurred.", errors.GetProperty("errors").GetProperty("IdNumber")[0].GetString());
    }

    [Fact]
    public async Task TestCase21()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST786"",
            ""firstName"": ""TIMMY"",
            ""lastName"": ""STONE"",
            ""middleName"": ""DAN"",
            ""suffix"": ""JR"",
            ""dob"": ""1980-01-01"",
            ""eyeColor"": ""BROWN"",
            ""gender"": ""male"",
            ""idType"": ""Driver License"",
            ""addressLine1"" : ""4301 Wilson Blvd"",
            ""addressLine2"": ""1234"",
            ""city"": ""Arlington"",
            ""state"": ""VA"",
            ""zip"": ""22203"",
            ""height"": ""510"",
            ""weight"": ""200"",
            ""expirationDate"": ""2023-10-10"",
            ""issueDate"": ""2011-10-10""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        _output.WriteLine($"Status Code: {response.StatusCode}");
        _output.WriteLine($"Body: {await response.Content.ReadAsStringAsync()}");
        Assert.True(response.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.False(matches.GetProperty("driverLicenseNumber").GetBoolean());

        Assert.False(matches.TryGetProperty("personLastNameExact", out _));
        Assert.False(matches.TryGetProperty("personFirstNameExact", out _));
        Assert.False(matches.TryGetProperty("personBirthDate", out _));
    }
}