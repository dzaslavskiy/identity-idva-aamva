using System.Text.Json;
using System.Text.Json.Nodes;

namespace Aamva.Configuration;

public class CFConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new CFConfigurationProvider();
    }
}

public class CFConfigurationProvider : ConfigurationProvider
{
    public override void Load()
    {
        var env = Environment.GetEnvironmentVariable("VCAP_SERVICES");

        if (env == null)
        {
            return;
        }

        JsonNode? json;
        try
        {
            json = JsonNode.Parse(env);
        }
        catch (JsonException)
        {
            return;
        }

        var creds = json?["user-provided"]?.AsArray()?.Single(x => ((string?)x?["name"]) == "aamva")?["credentials"]?.AsObject();
        if (creds == null)
        {
            return;
        }

        foreach (var cred in creds)
        {
            var value = (string?)cred.Value;
            if (value != null)
            {
                Data.Add(cred.Key, value);
            }
        }
    }
}

public static class CFExtensions
{
    public static IConfigurationBuilder AddCFConfiguration(
               this IConfigurationBuilder builder)
    {
        return builder.Add(new CFConfigurationSource());
    }
}