using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicResponse : MicModel
    {
        public IDictionary<string, object?> AdditionalData =>
            ((IMicModel)this).AdditionalData;
    }
}
