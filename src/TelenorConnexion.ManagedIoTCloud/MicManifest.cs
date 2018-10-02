using Amazon;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;
using THNETII.Common;
using THNETII.Common.Serialization;

namespace TelenorConnexion.ManagedIoTCloud
{
    /// <summary>
    /// Represents a MIC manifest document describing details of the various
    /// components that make up a MIC Stack Distribution.
    /// </summary>
    public partial class MicManifest
    {
        /// <summary>
        /// The AWS Lambda Function identifier for the Lambda returning the Manifest document.
        /// </summary>
        [JsonProperty(nameof(ManifestLambda))]
        public string ManifestLambda { get; set; }

        /// <summary>
        /// The Version of the MIC system currently in use.
        /// </summary>
        [JsonProperty("Rev")]
        public string Version { get; set; }

        [JsonProperty(nameof(ResponseLambda))]
        public string ResponseLambda { get; set; }

        /// <summary>
        /// The AWS Lambda Function identifier for the Permissions API.
        /// </summary>
        [JsonProperty(nameof(PermissionsLambda))]
        public string PermissionsLambda { get; set; }

        /// <summary>
        /// The Identity Pool Id for AWS Cognito Credentials.
        /// </summary>
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

        /// <summary>
        /// The AWS IoT Endpoint to use for MQTT connections.
        /// </summary>
        [JsonProperty(nameof(IotEndpoint))]
        public string IotEndpoint { get; set; }

        /// <summary>
        /// The AWS Lambda Function identifier for the Thing Batch API.
        /// </summary>
        [JsonProperty(nameof(ThingBatchLambda))]
        public string ThingBatchLambda { get; set; }

        /// <summary>
        /// The AWS Lambda function name to invoke for actions using the Auth API.
        /// </summary>
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

        /// <summary>
        /// The AWS Lambda function name to invoke for actions using the User API.
        /// </summary>
        [JsonProperty(nameof(UserLambda))]
        public string UserLambda { get; set; }

        /// <summary>
        /// The AWS Lambda function name to invoke for actions using the Observation API.
        /// </summary>
        [JsonProperty(nameof(ObservationLambda))]
        public string ObservationLambda { get; set; }

        [JsonProperty(nameof(GraphQLLambda))]
        public string GraphQLLambda { get; set; }

        [JsonProperty(nameof(ThingJobsTable))]
        public string ThingJobsTable { get; set; }

        /// <summary>
        /// The AWS Lambda function name to invoke for actions using the Event API.
        /// </summary>
        [JsonProperty(nameof(EventLambda))]
        public string EventLambda { get; set; }

        [JsonProperty(nameof(UnitTable))]
        public string UnitTable { get; set; }

        #region ApiGatewayRootUrl
        private readonly DuplexConversionTuple<string, Uri> apiGatewayRootUrl =
            GetUrlToUriDuplexConversionTuple();
        /// <summary>
        /// The API Gateway root URL-string for HTTP REST requests to the MIC API.
        /// </summary>
        /// <seealso cref="ApiGatewayRootUri"/>
        [JsonProperty(nameof(ApiGatewayRootUrl))]
        [SuppressMessage("Design", "CA1056:Uri properties should not be strings")]
        public string ApiGatewayRootUrl
        {
            get => apiGatewayRootUrl.RawValue;
            set => apiGatewayRootUrl.RawValue = value;
        }
        /// <summary>
        /// The API Gateway root URI for HTTP REST requests to the MIC API.
        /// </summary>
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

        #region Region
        private readonly DuplexConversionTuple<string, RegionEndpoint> region =
            new DuplexConversionTuple<string, RegionEndpoint>(
                rawConvert: s => RegionEndpoint.GetBySystemName(s),
                rawReverseConvert: r => r?.SystemName
                );
        /// <summary>
        /// The AWS system name for the region on which the MIC stack is deployed.
        /// </summary>
        [JsonProperty("Region")]
        public string RegionSystemName
        {
            get => region.RawValue;
            set => region.RawValue = value;
        }
        /// <summary>
        /// The AWS region endpoint to which the MIC stack is deployed.
        /// </summary>
        [JsonIgnore]
        public RegionEndpoint AwsRegion
        {
            get => region.ConvertedValue;
            set => region.ConvertedValue = value;
        }
        #endregion

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

        /// <summary>
        /// The AWS API Gateway Key Identifier for the API Key to use to
        /// authorise requests agains the MIC Cloud REST API.
        /// </summary>
        [JsonProperty(nameof(ApiKeyId))]
        public string ApiKeyId { get; set; }

        [JsonProperty(nameof(EsVersion))]
        public string EsVersion { get; set; }

        /// <summary>
        /// The AWS Account number that is used to deploy the MIC stack.
        /// </summary>
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

        /// <summary>
        /// The canonical name of the MIC stack that has been deployed.
        /// </summary>
        [JsonProperty(nameof(StackName))]
        public string StackName { get; set; }

        [JsonProperty(nameof(MqttFn))]
        public string MqttFn { get; set; }

        public string GetCognitoProviderName() =>
            $"cognito-idp.{RegionSystemName}.amazonaws.com/{UserPool}";

        private static DuplexConversionTuple<string, Uri> GetUrlToUriDuplexConversionTuple() =>
            new DuplexConversionTuple<string, Uri>(
                rawConvert: s => string.IsNullOrWhiteSpace(s) ? null : new Uri(s),
                rawReverseConvert: u => u?.ToString()
                );
    }
}
