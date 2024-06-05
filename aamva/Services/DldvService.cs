using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using Aamva.Models;
using Aamva.Utils;
using DldvServiceReference;
using static Aamva.Utils.SoapUtils;

namespace Aamva.Services;

public interface IDldvService
{
    public Task<MatchResponse> CallDldvService(ValidationRequest a);
}

public class DldvService : IDldvService
{
    private readonly ILogger<DldvService> _logger;
    private readonly IAuthService _authService;
    private readonly DLDVService21Client _client;
    private readonly string _origin;
    private readonly string _useAAMVATestSimulator;

    public DldvService(ILogger<DldvService> logger, IAuthService authService, IConfiguration config, IWebHostEnvironment hostEnv)
    {
        _logger = logger;
        _authService = authService;

        _origin = config["origin"];

        _useAAMVATestSimulator = config["UseAAMVATestSimulator"];

        var endpoint = config["soap_url"];

        _client = new DLDVService21Client(endpoint);

        if (hostEnv.IsDevelopment())
        {
            _client.ChannelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication()
            {
                CertificateValidationMode = X509CertificateValidationMode.None,
                RevocationMode = X509RevocationMode.NoCheck
            };
            _client.Endpoint.EndpointBehaviors.Add(new MessageLoggingBehavior(_logger));
        }
    }

    public async Task<MatchResponse> CallDldvService(ValidationRequest requestData)
    {
        _logger.LogDebug("CallDldvService");

        var destination = requestData.State ?? "";

        if (_useAAMVATestSimulator == "true")
        {
            destination = "P6";
        }

        var locatorId = Guid.NewGuid().ToString();

        var token = await _authService.GetToken();
        var request = ToSoapRequest(requestData, _origin, destination, locatorId);
        VerifyDriverLicenseDataResponse soapResponse;
        try
        {
            soapResponse = await _client.VerifyDriverLicenseDataAsync(token, request);
        }
        catch (FaultException<ExceptionType> exception)
        {
            Array.ForEach(exception.Detail.ProgramExceptions, x => _logger.LogError("Dldv FaultException: {} {}", x.ExceptionId, x.ExceptionText));
            throw;
        }
        var matchResponse = ToJsonResponse(soapResponse.VerifyDriverLicenseDataResult);
        return matchResponse;
    }

    public static VerifyDriverLicenseDataRequestType ToSoapRequest(ValidationRequest a, string origin, string destination, string locatorId)
    {
        return new VerifyDriverLicenseDataRequestType
        {
            ControlData = new ContolDataType
            {
                MessageAddress = new MessageAddressType
                {
                    MessageDestinationId = new TextType { Value = destination },
                    MessageOriginatorId = new TextType { Value = origin },
                    TransactionLocatorId = new TextType { Value = locatorId }
                }
            },
            Address = ToSoapAddress(a),
            DocumentCategoryCode = ToSoapDocCat(a.IdType),
            DriverLicenseExpirationDate = ToSoapDate(a.ExpirationDate),
            DriverLicenseIdentification = ToSoapId(a.IdNumber),
            DriverLicenseIssueDate = ToSoapDate(a.IssueDate),
            PersonBirthDate = ToSoapDate(a.Dob),
            PersonEyeColorCode = ToSoapEyeColor(a.EyeColor),
            PersonHeightMeasure = ToSoapInteger(a.Height),
            PersonName = ToSoapName(a),
            PersonSexCode = ToSoapSex(a.Gender),
            PersonWeightMeasure = ToSoapInteger(a.Weight),
        };
    }

    public static MatchResponse ToJsonResponse(VerifyDriverLicenseDataResponseType result)
    {
        var json = new MatchResponse
        {
            LocatorId = result.ControlData.MessageAddress.TransactionLocatorId.Value,

            DriverLicenseNumber = result.DriverLicenseNumberMatchIndicator,
            DriverLicenseExpirationDate = result.DriverLicenseExpirationDateMatchIndicatorSpecified ? result.DriverLicenseExpirationDateMatchIndicator : null,
            DriverLicenseIssueDate = result.DriverLicenseIssueDateMatchIndicatorSpecified ? result.DriverLicenseIssueDateMatchIndicator : null,

            DocumentCategory = result.DocumentCategoryMatchIndicatorSpecified ? result.DocumentCategoryMatchIndicator : null,

            PersonLastNameExact = result.PersonLastNameExactMatchIndicatorSpecified ? result.PersonLastNameExactMatchIndicator : null,
            PersonLastNameFuzzyPrimary = result.PersonLastNameFuzzyPrimaryMatchIndicatorSpecified ? result.PersonLastNameFuzzyPrimaryMatchIndicator : null,
            PersonLastNameFuzzyAlternate = result.PersonLastNameFuzzyAlternateMatchIndicatorSpecified ? result.PersonLastNameFuzzyAlternateMatchIndicator : null,
            PersonFirstNameExact = result.PersonFirstNameExactMatchIndicatorSpecified ? result.PersonFirstNameExactMatchIndicator : null,
            PersonFirstNameFuzzyPrimary = result.PersonFirstNameFuzzyPrimaryMatchIndicatorSpecified ? result.PersonFirstNameFuzzyPrimaryMatchIndicator : null,
            PersonFirstNameFuzzyAlternate = result.PersonFirstNameFuzzyAlternateMatchIndicatorSpecified ? result.PersonFirstNameFuzzyAlternateMatchIndicator : null,
            PersonMiddleNameExact = result.PersonMiddleNameExactMatchIndicatorSpecified ? result.PersonMiddleNameExactMatchIndicator : null,
            PersonMiddleNameFuzzyPrimary = result.PersonMiddleNameFuzzyPrimaryMatchIndicatorSpecified ? result.PersonMiddleNameFuzzyPrimaryMatchIndicator : null,
            PersonMiddleNameFuzzyAlternate = result.PersonMiddleNameFuzzyAlternateMatchIndicatorSpecified ? result.PersonMiddleNameFuzzyAlternateMatchIndicator : null,
            PersonMiddleInitial = result.PersonMiddleInitialMatchIndicatorSpecified ? result.PersonMiddleInitialMatchIndicator : null,
            PersonNameSuffix = result.PersonNameSuffixMatchIndicatorSpecified ? result.PersonNameSuffixMatchIndicator : null,

            PersonBirthDate = result.PersonBirthDateMatchIndicatorSpecified ? result.PersonBirthDateMatchIndicator : null,
            PersonSexCode = result.PersonSexCodeMatchIndicatorSpecified ? result.PersonSexCodeMatchIndicator : null,

            PersonHeight = result.PersonHeightMatchIndicatorSpecified ? result.PersonHeightMatchIndicator : null,
            PersonWeight = result.PersonWeightMatchIndicatorSpecified ? result.PersonWeightMatchIndicator : null,
            PersonEyeColor = result.PersonEyeColorMatchIndicatorSpecified ? result.PersonEyeColorMatchIndicator : null,

            AddressLine1 = result.AddressLine1MatchIndicatorSpecified ? result.AddressLine1MatchIndicator : null,
            AddressLine2 = result.AddressLine2MatchIndicatorSpecified ? result.AddressLine2MatchIndicator : null,
            AddressCity = result.AddressCityMatchIndicatorSpecified ? result.AddressCityMatchIndicator : null,
            AddressStateCode = result.AddressStateCodeMatchIndicatorSpecified ? result.AddressStateCodeMatchIndicator : null,
            AddressZIP5 = result.AddressZIP5MatchIndicatorSpecified ? result.AddressZIP5MatchIndicator : null,
            AddressZIP4 = result.AddressZIP4MatchIndicatorSpecified ? result.AddressZIP4MatchIndicator : null
        };

        return json;
    }
}