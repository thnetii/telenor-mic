using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicUserGetRequest : MicUserBasicInfo
    {
        public MicUserDataAttributes? Attributes { get; set; }
    }
}
