using Amazon;
using Newtonsoft.Json;
using THNETII.Common;

namespace TelenorConnexion.ManagedIoTCloud.CloudApi.Model
{
    public class MicMetadataManifest : MicModel
    {
        [JsonProperty]
        public string? ApiKey { get; set; }
        [JsonProperty]
        public string? IotEndpoint { get; set; }
        [JsonProperty]
        public string? IdentityPool { get; set; }
        [JsonProperty]
        public string? UserPool { get; set; }
        #region Region
        private readonly DuplexConversionTuple<string?, RegionEndpoint?> region =
            new DuplexConversionTuple<string?, RegionEndpoint?>(
                rawConvert: s => s is null ? null : RegionEndpoint.GetBySystemName(s),
                rawReverseConvert: r => r?.SystemName
                );
        /// <summary>
        /// The AWS system name for the region on which the MIC stack is deployed.
        /// </summary>
        [JsonProperty("Region")]
        public string? RegionSystemName
        {
            get => region.RawValue;
            set => region.RawValue = value;
        }
        /// <summary>
        /// The AWS region endpoint to which the MIC stack is deployed.
        /// </summary>
        [JsonIgnore]
        public RegionEndpoint? AwsRegion
        {
            get => region.ConvertedValue;
            set => region.ConvertedValue = value;
        }
        #endregion
    }
}
