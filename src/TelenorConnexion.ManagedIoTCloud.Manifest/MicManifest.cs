using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using THNETII.Common;
using THNETII.Common.Serialization;

namespace TelenorConnexion.ManagedIoTCloud
{
    public partial class MicManifest
    {
        [JsonProperty(nameof(ManifestLambda))]
        public string ManifestLambda { get; set; }
        [JsonProperty("Rev")]
        public string Version { get; set; }
        [JsonProperty(nameof(ResponseLambda))]
        public string ResponseLambda { get; set; }
        [JsonProperty(nameof(PermissionsLambda))]
        public string PermissionsLambda { get; set; }
        [JsonProperty(nameof(IdentityPool))]
        public string IdentityPool { get; set; }
        #region LogLevel
        private readonly DuplexConversionTuple<string, int> logLevel =
            new DuplexConversionTuple<string, int>(
                rawConvert: s => int.TryParse(s, out int l) ? l : 0,
                rawReverseConvert: l => l.ToString(CultureInfo.InvariantCulture)
            );
        [JsonProperty("LogLevel")]
        public string LogLevelString
        {
            get => logLevel.RawValue;
            set => logLevel.RawValue = value;
        }
        [IgnoreDataMember]
        public int LogLevel
        {
            get => logLevel.ConvertedValue;
            set => logLevel.ConvertedValue = value;
        }
        #endregion
        [JsonProperty(nameof(IotEndpoint))]
        public string IotEndpoint { get; set; }
        [JsonProperty(nameof(ThingBatchLambda))]
        public string ThingBatchLambda { get; set; }
        [JsonProperty(nameof(AuthLambda))]
        public string AuthLambda { get; set; }
        [JsonProperty(nameof(Permissions))]
        public string Permissions { get; set; }
        #region ThingEvent
        private readonly DuplexConversionTuple<string, Uri> thingEvent =
            GetUrlToUriDuplexConversionTuple();
        [JsonProperty("ThingEvent")]
        [SuppressMessage("Design", "CA1056:Uri properties should not be strings")]
        public string ThingEventUrl
        {
            get => thingEvent.RawValue;
            set => thingEvent.RawValue = value;
        }
        [IgnoreDataMember]
        public Uri ThingEventUri
        {
            get => thingEvent.ConvertedValue;
            set => thingEvent.ConvertedValue = value;
        }
        #endregion
        [JsonProperty(nameof(UserPoolClient))]
        public string UserPoolClient { get; set; }
        [JsonProperty(nameof(RulesTable))]
        public string RulesTable { get; set; }
        [JsonProperty(nameof(ResourceTable))]
        public string ResourceTable { get; set; }
        [JsonProperty(nameof(UserLambda))]
        public string UserLambda { get; set; }
        [JsonProperty(nameof(ObservationLambda))]
        public string ObservationLambda { get; set; }
        [JsonProperty(nameof(GraphQLLambda))]
        public string GraphQLLambda { get; set; }
        [JsonProperty(nameof(ThingJobsTable))]
        public string ThingJobsTable { get; set; }
        [JsonProperty(nameof(EventLambda))]
        public string EventLambda { get; set; }
        [JsonProperty(nameof(UnitTable))]
        public string UnitTable { get; set; }
        #region ApiGatewayRootUrl
        private readonly DuplexConversionTuple<string, Uri> apiGatewayRootUrl =
            GetUrlToUriDuplexConversionTuple();
        [JsonProperty(nameof(ApiGatewayRootUrl))]
        [SuppressMessage("Design", "CA1056:Uri properties should not be strings")]
        public string ApiGatewayRootUrl
        {
            get => apiGatewayRootUrl.RawValue;
            set => apiGatewayRootUrl.RawValue = value;
        }
        [IgnoreDataMember]
        public Uri ApiGatewayRootUri
        {
            get => apiGatewayRootUrl.ConvertedValue;
            set => apiGatewayRootUrl.ConvertedValue = value;
        }
        #endregion
        [JsonProperty(nameof(Es5Endpoint))]
        public string Es5Endpoint { get; set; }
        [JsonProperty(nameof(FileLambda))]
        public string FileLambda { get; set; }
        [JsonProperty(nameof(ManagementLambda))]
        public string ManagementLambda { get; set; }
        [JsonProperty("Region")]
        public string AwsRegion { get; set; }
        [JsonProperty(nameof(ThingLambda))]
        public string ThingLambda { get; set; }
        [JsonProperty(nameof(ThingTypeLambda))]
        public string ThingTypeLambda { get; set; }
        [JsonProperty(nameof(PermissionsTable))]
        public string PermissionsTable { get; set; }
        [JsonProperty(nameof(AtomicCountersTable))]
        public string AtomicCountersTable { get; set; }
        [JsonProperty(nameof(DashboardLambda))]
        public string DashboardLambda { get; set; }
        [JsonProperty(nameof(DomainLambda))]
        public string DomainLambda { get; set; }
        [JsonProperty(nameof(ObservationsBucket))]
        public string ObservationsBucket { get; set; }
        [JsonProperty(nameof(ThingTypesTable))]
        public string ThingTypesTable { get; set; }
        [JsonProperty(nameof(SignUpVerificationMedium))]
        public string SignUpVerificationMedium { get; set; }
        [JsonProperty(nameof(ThingGroupsLambda))]
        public string ThingGroupsLambda { get; set; }
        [JsonProperty(nameof(UsersTable))]
        public string UsersTable { get; set; }
        [JsonProperty(nameof(LoraLambda))]
        public string LoraLambda { get; set; }
        [JsonProperty(nameof(UserPool))]
        public string UserPool { get; set; }
        #region ConsentRequired
        private readonly DuplexConversionTuple<string, bool> consentRequired =
            new DuplexConversionTuple<string, bool>(
                s => BooleanStringConverter.TryParse(s, out bool b) ? b : false,
                b => BooleanStringConverter.ToString(b)
                );
        [JsonProperty("ConsentRequired")]
        public string ConsentRequiredText
        {
            get => consentRequired.RawValue;
            set => consentRequired.RawValue = value;
        }
        [IgnoreDataMember]
        public bool ConsentRequired
        {
            get => consentRequired.ConvertedValue;
            set => consentRequired.ConvertedValue = value;
        }
        #endregion
        [JsonProperty(nameof(DomainTreeTable))]
        public string DomainTreeTable { get; set; }
        [JsonProperty(nameof(ThingFilesBucket))]
        public string ThingFilesBucket { get; set; }
        [JsonProperty(nameof(ThingCertsBucket))]
        public string ThingCertsBucket { get; set; }
        [JsonProperty(nameof(SmsLambda))]
        public string SmsLambda { get; set; }
        [JsonProperty(nameof(RuleLambda))]
        public string RuleLambda { get; set; }
        [JsonProperty(nameof(SearchLambda))]
        public string SearchLambda { get; set; }
        [JsonProperty(nameof(FileLambdaV2))]
        public string FileLambdaV2 { get; set; }
        [JsonProperty(nameof(ThingJobsLambda))]
        public string ThingJobsLambda { get; set; }
        [JsonProperty(nameof(Protocol))]
        public string Protocol { get; set; }
        [JsonProperty(nameof(DashboardTable))]
        public string DashboardTable { get; set; }
        [JsonProperty(nameof(ApiKeyId))]
        public string ApiKeyId { get; set; }
        [JsonProperty(nameof(EsVersion))]
        public string EsVersion { get; set; }
        [JsonProperty(nameof(AccountNumber))]
        public string AccountNumber { get; set; }
        [JsonProperty(nameof(ResourceLambda))]
        public string ResourceLambda { get; set; }
        [JsonProperty(nameof(ResourceOptionsTable))]
        public string ResourceOptionsTable { get; set; }
        [JsonProperty(nameof(PublicBucket))]
        public string PublicBucket { get; set; }
        [JsonProperty(nameof(UserDataTable))]
        public string UserDataTable { get; set; }
        [JsonProperty(nameof(GraphIQLLambda))]
        public string GraphIQLLambda { get; set; }
        [JsonProperty(nameof(ApiId))]
        public string ApiId { get; set; }
        [JsonProperty(nameof(StackName))]
        public string StackName { get; set; }
        [JsonProperty(nameof(MqttFn))]
        public string MqttFn { get; set; }

        private static DuplexConversionTuple<string, Uri> GetUrlToUriDuplexConversionTuple() =>
            new DuplexConversionTuple<string, Uri>(
                rawConvert: s => string.IsNullOrWhiteSpace(s) ? null : new Uri(s),
                rawReverseConvert: u => u?.ToString()
                );
    }
}
