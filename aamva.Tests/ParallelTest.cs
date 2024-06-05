using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aamva.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Aamva.Tests;

public class ParallelTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _output;

    public ParallelTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
    }

    [Fact]
    public async Task Parallel()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST11"",
            ""firstName"": ""STEVEN"",
            ""lastName"": ""BROWN"",
            ""dob"": ""1980-01-01""
        }";

        var request1 = client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        var request2 = client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        var request3 = client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        var request4 = client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));
        var tasks = await Task.WhenAll(request1, request2, request3, request4);
        var response = tasks[0];
        var response2 = tasks[1];
        var response3 = tasks[2];
        var response4 = tasks[3];
        Assert.True(response.IsSuccessStatusCode);
        Assert.True(response2.IsSuccessStatusCode);
        Assert.True(response3.IsSuccessStatusCode);
        Assert.True(response4.IsSuccessStatusCode);

        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());
    }

    [Fact]
    public async Task Single()
    {
        var client = _factory.CreateClient();

        var s = @"
        {
            ""idNumber"": ""DLDVSTRUCTUREDTEST11"",
            ""firstName"": ""STEVEN"",
            ""lastName"": ""BROWN"",
            ""dob"": ""1980-01-01""
        }";

        var request1 = client.PostAsync("/aamva", new StringContent(s, Encoding.UTF8, "application/json"));

        var response = await request1;

        Assert.True(response.IsSuccessStatusCode);


        var matches = await response.Content.ReadFromJsonAsync<JsonElement>();
        _output.WriteLine(matches.ToStringPretty());

        Assert.True(Guid.TryParse(matches.GetProperty("locatorId").GetString(), out _));

        Assert.True(matches.GetProperty("driverLicenseNumber").GetBoolean());
        Assert.True(matches.GetProperty("personLastNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personFirstNameExact").GetBoolean());
        Assert.True(matches.GetProperty("personBirthDate").GetBoolean());
    }


}