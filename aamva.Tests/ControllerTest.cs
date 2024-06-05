using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aamva.Services;
using Aamva.Tests.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Aamva.Tests;

public class AamvaTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AamvaTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IAuthService, TestAuthService>();
                services.AddSingleton<IDldvService, TestDldvService>();
            });
        });
    }

    [Fact]
    public async Task Post_Valid_ReturnValid()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""firstName"": ""John"",
            ""lastName"": ""Smith"",
            ""middleName"": ""(joe)"",
            ""suffix"": ""Jr."",

            ""dob"": ""1902-01-01"",
            ""gender"": ""male"",

            ""addressLine1"": ""1002 Main st."",
            ""addressLine2"": ""Apt #202"",
            ""city"": ""Carralton"",
            ""state"": ""Va"",
            ""zip"": ""20202"",

            ""idNumber"": ""8923498273432"",
            ""issueDate"": ""2020-01-01"",
            ""expirationDate"": ""2023-01-01"",

            ""idType"": ""driver license"",

            ""weight"": ""100"",
            ""eyeColor"": ""green"",
            ""height"": ""510""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(response.IsSuccessStatusCode);
        var locatorId = matches.GetProperty("locatorId").GetString();
        Assert.True(Guid.TryParse(locatorId, out _));
    }

    [Fact]
    public async Task Post_MissingId_ReturnError()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var numErrors = body.GetProperty("errors").EnumerateObject().Count();
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(1, numErrors);
    }

    [Fact]
    public async Task Post_InvalidData_ReturnValidationError()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""8923498273432"",

            ""dob"": ""asdf"",
            ""gender"": ""fire"",
            ""state"": ""Zz"",
            ""issueDate"": ""future"",
            ""expirationDate"": ""past"",
            ""idType"": ""none"",
            ""eyeColor"": ""orange""
        }";

        var response = await client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var numErrors = body.GetProperty("errors").EnumerateObject().Count();
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(7, numErrors);
    }
}
