using System;
using System.Threading.Tasks;
using Aamva.Models;
using Aamva.Services;

namespace Aamva.Tests.Services;

public class TestAuthService : IAuthService
{
    public Task<byte[]> GetToken()
    {
        return Task.FromResult(Array.Empty<byte>());
    }
}

public class TestDldvService : IDldvService
{
    public Task<MatchResponse> CallDldvService(ValidationRequest a)
    {
        var locatorId = Guid.NewGuid().ToString();
        return Task.FromResult(new MatchResponse
        {
            LocatorId = locatorId
        });
    }
}