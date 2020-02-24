using System;
using System.Diagnostics.CodeAnalysis;
#if NETSTANDARD1_3
using System.Reflection;
#endif

using Amazon.Runtime;
using Amazon.Util.Internal;

#nullable disable
namespace TelenorConnexion.ManagedIoTCloud
{
    public class MicClientConfig : ClientConfig
    {
        public const string ServiceName = "tcxn-mic";
        private static readonly string version = typeof(MicClientConfig)
#if NETSTANDARD1_3
            .GetTypeInfo()
#endif
            .Assembly.GetName().Version.ToString();

        public override string ServiceVersion { get; } = version;
        public override string UserAgent { get; } = InternalSDKUtils.BuildUserAgentString(version);
        public override string RegionEndpointServiceName { get; } = ServiceName;

        /// <summary>
        /// Gets or sets the MIC Stack Hostname to use.
        /// </summary>
        public string Hostname { get; set; }

        [SuppressMessage("Usage", "CA2208: Instantiate argument exceptions correctly")]
        public override void Validate()
        {
            _ = Hostname ?? throw new ArgumentNullException(nameof(Hostname));
        }
    }
}
