using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Aamva.Utils;

public static class DebugUtils
{
    public static void PrintProps(ILogger log, object obj, string prefix = "")
    {
        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
        {
            var name = descriptor.Name;
            var value = descriptor.GetValue(obj);
            log.LogWarning("{prefix}: {name}={value}", prefix, name, value);
            if (value != null && value.GetType() != obj.GetType())
            {
                PrintProps(log, value, prefix);
            }
        }
    }

    public static async Task<string> PrintBody(HttpRequest request)
    {
        return await new StreamReader(request.Body).ReadToEndAsync();
    }
}

public static partial class JsonExtensions
{
    public static string ToStringPretty(this JsonElement element, bool indent = true)
        => element.ValueKind == JsonValueKind.Undefined ? "" : JsonSerializer.Serialize(element, new JsonSerializerOptions { WriteIndented = indent });

    public static string ToStringIndented(this JsonElement value)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        value.WriteTo(writer);
        writer.Flush();
        var indented = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);

        return indented;
    }
}

public class MessageLoggingBehavior : IEndpointBehavior
{
    public MessageLoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    private readonly ILogger _logger;

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
        clientRuntime.ClientMessageInspectors.Add(new MessageLoggingInspector(_logger));
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }
}

public class MessageLoggingInspector : IClientMessageInspector
{
    private readonly ILogger _logger;

    public MessageLoggingInspector(ILogger logger)
    {
        _logger = logger;
    }

    public object BeforeSendRequest(ref Message request, IClientChannel channel)
    {
        var correlationId = Guid.NewGuid();
        _logger.LogDebug("SOAP Request: {} {}", correlationId.ToString(), request.ToString());
        return correlationId;
    }

    public void AfterReceiveReply(ref Message reply, object correlationState)
    {
        var correlationId = (Guid)correlationState;
        _logger.LogDebug("SOAP Reply: {} {}", correlationId.ToString(), reply.ToString());
    }
}