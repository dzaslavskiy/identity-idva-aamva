using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Aamva.Services;

public interface IAuthService
{
    public Task<byte[]> GetToken();
}

public class AuthService : IAuthService
{
    private readonly ILogger<DldvService> _logger;
    private readonly HttpClient _httpclient;
    private readonly IConfiguration _config;

    private byte[] _authToken;
    private DateTime _expiration;
    private SemaphoreSlim semaphore;

    private readonly TimeSpan _tokenExpirationBuffer = TimeSpan.FromMinutes(5);

    public AuthService(ILogger<DldvService> logger, IWebHostEnvironment env, IConfiguration config)
    {
        semaphore = new SemaphoreSlim(0, 1);
        _logger = logger;
        _config = config;

        X509Certificate cert;

        if (env.IsDevelopment())
        {
            var certPath = Path.Combine(env.ContentRootPath, _config["cert_filename"]);
            _logger.LogDebug("Loading client cert from file: {}", certPath);
            cert = new X509Certificate(certPath, _config["cert_password"]);
        }
        else
        {
            _logger.LogDebug("Loading client cert from cf user service: {}", "aamva");
            var data = Convert.FromBase64String(_config["cert_data"]);
            cert = new X509Certificate(data, _config["cert_password"]);
        }

        var handler = new SocketsHttpHandler();
        handler.SslOptions.ClientCertificates ??= new X509CertificateCollection();
        handler.SslOptions.ClientCertificates.Add(cert);

        _httpclient = new HttpClient(handler);

        _expiration = DateTime.MinValue;
        GetToken().GetAwaiter().GetResult();
    }

    public async Task<byte[]> GetToken()
    {
        // TODO: make code better
        /*
            if valid & not being updated
                return token
            if invalid & not being updated
                update
                return token
            if invalid & being updated
                wait
                return token
        */
        try
        {
            //await semaphore.WaitAsync();
            //semaphore.Wait();
            if (_expiration < DateTime.Now)
            {
                _logger.LogInformation("Token expired");
                var result = await _httpclient.GetFromJsonAsync<TokenResult>(_config["auth_url"]);
                _authToken = Convert.FromBase64String(result.EncValue);
                _expiration = DateTime.Now.AddMinutes(result.TimeToLiveinMins).Subtract(_tokenExpirationBuffer);
                _logger.LogInformation("Refreshed token, expires {}", _expiration);
            }
            //semaphore.Release();
            return _authToken;
        }
        finally
        {

        }
    }

    private class TokenResult
    {
        public string EncValue { get; set; }
        public int TimeToLiveinMins { get; set; }
    }
}