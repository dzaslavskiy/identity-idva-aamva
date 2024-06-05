using System.ServiceModel;

namespace DldvServiceReference;

public partial class DLDVService21Client
{
    public DLDVService21Client(string remoteAddress) :
        this(GetDefaultBinding(), new EndpointAddress(remoteAddress))
    {
    }
}