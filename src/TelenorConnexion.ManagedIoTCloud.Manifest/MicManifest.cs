using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using THNETII.Common;
using THNETII.Common.Serialization;

namespace TelenorConnexion.ManagedIoTCloud
{
    [DataContract]
    public partial class MicManifest
    {
        [DataMember(Name = nameof(ManifestLambda))]
        public string ManifestLambda { get; set; }
        [DataMember(Name = "Rev")]
        public string Version { get; set; }
        [DataMember(Name = nameof(ResponseLambda))]
        public string ResponseLambda { get; set; }
        [DataMember(Name = nameof(PermissionsLambda))]
        public string PermissionsLambda { get; set; }
        [DataMember(Name = nameof(IdentityPool))]
        public string IdentityPool { get; set; }
        #region LogLevel
        private readonly DuplexConversionTuple<string, int> logLevel =
            new DuplexConversionTuple<string, int>(
                rawConvert: s => int.TryParse(s, out int l) ? l : 0,
                rawReverseConvert: l => l.ToString(CultureInfo.InvariantCulture)
            );
        [DataMember(Name = "LogLevel")]
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
        [DataMember(Name = nameof(IotEndpoint))]
        public string IotEndpoint { get; set; }
        [DataMember(Name = nameof(ThingBatchLambda))]
        public string ThingBatchLambda { get; set; }
        [DataMember(Name = nameof(AuthLambda))]
        public string AuthLambda { get; set; }
        [DataMember(Name = nameof(Permissions))]
        public string Permissions { get; set; }
        #region ThingEvent
        private readonly DuplexConversionTuple<string, Uri> thingEvent =
            GetUrlToUriDuplexConversionTuple();
        [DataMember(Name = "ThingEvent")]
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
        [DataMember(Name = nameof(UserPoolClient))]
        public string UserPoolClient { get; set; }
        [DataMember(Name = nameof(RulesTable))]
        public string RulesTable { get; set; }
        [DataMember(Name = nameof(ResourceTable))]
        public string ResourceTable { get; set; }
        [DataMember(Name = nameof(UserLambda))]
        public string UserLambda { get; set; }
        [DataMember(Name = nameof(ObservationLambda))]
        public string ObservationLambda { get; set; }
        [DataMember(Name = nameof(GraphQLLambda))]
        public string GraphQLLambda { get; set; }
        [DataMember(Name = nameof(ThingJobsTable))]
        public string ThingJobsTable { get; set; }
        [DataMember(Name = nameof(EventLambda))]
        public string EventLambda { get; set; }
        [DataMember(Name = nameof(UnitTable))]
        public string UnitTable { get; set; }
        #region ApiGatewayRootUrl
        private readonly DuplexConversionTuple<string, Uri> apiGatewayRootUrl =
            GetUrlToUriDuplexConversionTuple();
        [DataMember(Name = nameof(ApiGatewayRootUrl))]
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
        [DataMember(Name = nameof(Es5Endpoint))]
        public string Es5Endpoint { get; set; }
        [DataMember(Name = nameof(FileLambda))]
        public string FileLambda { get; set; }
        [DataMember(Name = nameof(ManagementLambda))]
        public string ManagementLambda { get; set; }
        [DataMember(Name = "Region")]
        public string AwsRegion { get; set; }
        [DataMember(Name = nameof(ThingLambda))]
        public string ThingLambda { get; set; }
        [DataMember(Name = nameof(ThingTypeLambda))]
        public string ThingTypeLambda { get; set; }
        [DataMember(Name = nameof(PermissionsTable))]
        public string PermissionsTable { get; set; }
        [DataMember(Name = nameof(AtomicCountersTable))]
        public string AtomicCountersTable { get; set; }
        [DataMember(Name = nameof(DashboardLambda))]
        public string DashboardLambda { get; set; }
        [DataMember(Name = nameof(DomainLambda))]
        public string DomainLambda { get; set; }
        [DataMember(Name = nameof(ObservationsBucket))]
        public string ObservationsBucket { get; set; }
        [DataMember(Name = nameof(ThingTypesTable))]
        public string ThingTypesTable { get; set; }
        [DataMember(Name = nameof(SignUpVerificationMedium))]
        public string SignUpVerificationMedium { get; set; }
        [DataMember(Name = nameof(ThingGroupsLambda))]
        public string ThingGroupsLambda { get; set; }
        [DataMember(Name = nameof(UsersTable))]
        public string UsersTable { get; set; }
        [DataMember(Name = nameof(LoraLambda))]
        public string LoraLambda { get; set; }
        [DataMember(Name = nameof(UserPool))]
        public string UserPool { get; set; }
        #region ConsentRequired
        private readonly DuplexConversionTuple<string, bool> consentRequired =
            new DuplexConversionTuple<string, bool>(
                s => BooleanStringConverter.TryParse(s, out bool b) ? b : false,
                b => BooleanStringConverter.ToString(b)
                );
        [DataMember(Name = "ConsentRequired")]
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
        [DataMember(Name = nameof(DomainTreeTable))]
        public string DomainTreeTable { get; set; }
        [DataMember(Name = nameof(ThingFilesBucket))]
        public string ThingFilesBucket { get; set; }
        [DataMember(Name = nameof(ThingCertsBucket))]
        public string ThingCertsBucket { get; set; }
        [DataMember(Name = nameof(SmsLambda))]
        public string SmsLambda { get; set; }
        [DataMember(Name = nameof(RuleLambda))]
        public string RuleLambda { get; set; }
        [DataMember(Name = nameof(SearchLambda))]
        public string SearchLambda { get; set; }
        [DataMember(Name = nameof(FileLambdaV2))]
        public string FileLambdaV2 { get; set; }
        [DataMember(Name = nameof(ThingJobsLambda))]
        public string ThingJobsLambda { get; set; }
        [DataMember(Name = nameof(Protocol))]
        public string Protocol { get; set; }
        [DataMember(Name = nameof(DashboardTable))]
        public string DashboardTable { get; set; }
        [DataMember(Name = nameof(ApiKeyId))]
        public string ApiKeyId { get; set; }
        [DataMember(Name = nameof(EsVersion))]
        public string EsVersion { get; set; }
        [DataMember(Name = nameof(AccountNumber))]
        public string AccountNumber { get; set; }
        [DataMember(Name = nameof(ResourceLambda))]
        public string ResourceLambda { get; set; }
        [DataMember(Name = nameof(ResourceOptionsTable))]
        public string ResourceOptionsTable { get; set; }
        [DataMember(Name = nameof(PublicBucket))]
        public string PublicBucket { get; set; }
        [DataMember(Name = nameof(UserDataTable))]
        public string UserDataTable { get; set; }
        [DataMember(Name = nameof(GraphIQLLambda))]
        public string GraphIQLLambda { get; set; }
        [DataMember(Name = nameof(ApiId))]
        public string ApiId { get; set; }
        [DataMember(Name = nameof(StackName))]
        public string StackName { get; set; }
        [DataMember(Name = nameof(MqttFn))]
        public string MqttFn { get; set; }

        private static DuplexConversionTuple<string, Uri> GetUrlToUriDuplexConversionTuple() =>
            new DuplexConversionTuple<string, Uri>(
                rawConvert: s => string.IsNullOrWhiteSpace(s) ? null : new Uri(s),
                rawReverseConvert: u => u?.ToString()
                );
    }
}
