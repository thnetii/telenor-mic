using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.Model
{
    public class MicResponse : MicModel
    {
        public IDictionary<string, object> AdditionalData =>
            ((IMicModel)this).AdditionalData;
    }
}
