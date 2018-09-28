﻿using Amazon;
using Amazon.Runtime;
using Amazon.Util.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace TelenorConnexion.ManagedIoTCloud
{
    public class MicConfig : ClientConfig
    {
        private static IEnumerable<ClientConfig> NoopClientAccessor() =>
            Enumerable.Empty<ClientConfig>();

        private readonly Func<IEnumerable<ClientConfig>> clientConfigAccessor = NoopClientAccessor;

        internal MicConfig(Func<IEnumerable<ClientConfig>> clientConfigAccessor) : this()
        {
            this.clientConfigAccessor = clientConfigAccessor ??
                throw new ArgumentNullException(nameof(clientConfigAccessor));
        }

        #region Redefined Members

        /// <inheritdoc />
        public new SigningAlgorithm SignatureMethod
        {
            get => base.SignatureMethod;
            set => SetProperty(nameof(base.SignatureMethod), value);
        }

        /// <inheritdoc />
        public new string SignatureVersion
        {
            get => base.SignatureVersion;
            set => SetProperty(nameof(base.SignatureVersion), value);
        }

        /// <inheritdoc />
        public new RegionEndpoint RegionEndpoint
        {
            get => base.RegionEndpoint;
            set => SetProperty(nameof(base.RegionEndpoint), value);
        }

        /// <inheritdoc />
        public new string ServiceURL
        {
            get => base.ServiceURL;
            set => SetProperty(nameof(base.ServiceURL), value);
        }

        /// <inheritdoc />
        public new bool UseHttp
        {
            get => base.UseHttp;
            set => SetProperty(nameof(base.UseHttp), value);
        }

        /// <inheritdoc />
        public new string AuthenticationRegion
        {
            get => base.AuthenticationRegion;
            set => SetProperty(nameof(base.AuthenticationRegion), value);
        }

        /// <inheritdoc />
        public new string AuthenticationServiceName
        {
            get => base.AuthenticationServiceName;
            set => SetProperty(nameof(base.AuthenticationServiceName), value);
        }

        /// <inheritdoc />
        public new int MaxErrorRetry
        {
            get => base.MaxErrorRetry;
            set => SetProperty(nameof(base.MaxErrorRetry), value);
        }

        /// <inheritdoc />
        public new bool LogResponse
        {
            get => base.LogResponse;
            set => SetProperty(nameof(base.LogResponse), value);
        }

        /// <inheritdoc />
        [Obsolete("This property does not effect response processing and is deprecated.To enable response logging, the ClientConfig.LogResponse and AWSConfigs.LoggingConfig.LogResponses properties can be used.")]
        public new bool ReadEntireResponse
        {
            get => base.ReadEntireResponse;
            set => SetProperty(nameof(base.ReadEntireResponse), value);
        }

        /// <inheritdoc />
        public new int BufferSize
        {
            get => base.BufferSize;
            set => SetProperty(nameof(base.BufferSize), value);
        }

        /// <inheritdoc />
        public new long ProgressUpdateInterval
        {
            get => base.ProgressUpdateInterval;
            set => SetProperty(nameof(base.ProgressUpdateInterval), value);
        }

        /// <inheritdoc />
        public new bool ResignRetries
        {
            get => base.ResignRetries;
            set => SetProperty(nameof(base.ResignRetries), value);
        }

        /// <inheritdoc />
        public new bool AllowAutoRedirect
        {
            get => base.AllowAutoRedirect;
            set => SetProperty(nameof(base.AllowAutoRedirect), value);
        }

        /// <inheritdoc />
        public new bool LogMetrics
        {
            get => base.LogMetrics;
            set => SetProperty(nameof(base.LogMetrics), value);
        }

        /// <inheritdoc />
        public new bool DisableLogging
        {
            get => base.DisableLogging;
            set => SetProperty(nameof(base.DisableLogging), value);
        }

        /// <inheritdoc />
        public new ICredentials ProxyCredentials
        {
            get => base.ProxyCredentials;
            set => SetProperty(nameof(base.ProxyCredentials), value);
        }

        /// <inheritdoc />
        public new TimeSpan? Timeout
        {
            get => base.Timeout;
            set => SetProperty(nameof(base.Timeout), value);
        }

        /// <inheritdoc />
        public new bool UseDualstackEndpoint
        {
            get => base.UseDualstackEndpoint;
            set => SetProperty(nameof(base.UseDualstackEndpoint), value);
        }

        /// <inheritdoc />
        public new bool ThrottleRetries
        {
            get => base.ThrottleRetries;
            set => SetProperty(nameof(base.ThrottleRetries), value);
        }

        /// <inheritdoc />
        public new bool CacheHttpClient
        {
            get => base.CacheHttpClient;
            set => SetProperty(nameof(base.CacheHttpClient), value);
        }

        /// <inheritdoc />
        public new int HttpClientCacheSize
        {
            get => base.HttpClientCacheSize;
            set => SetProperty(nameof(base.HttpClientCacheSize), value);
        }

        /// <inheritdoc />
        public new TimeSpan? ReadWriteTimeout
        {
            get => base.ReadWriteTimeout;
            set => SetProperty(nameof(base.ReadWriteTimeout), value);
        }

        /// <inheritdoc />
        public new string ProxyHost
        {
            get => base.ProxyHost;
            set => SetProperty(nameof(base.ProxyHost), value);
        }

        /// <inheritdoc />
        public new int ProxyPort
        {
            get => base.ProxyPort;
            set => SetProperty(nameof(base.ProxyPort), value);
        }

        /// <inheritdoc />
        public new int? MaxConnectionsPerServer
        {
            get => base.MaxConnectionsPerServer;
            set => SetProperty(nameof(base.MaxConnectionsPerServer), value);
        }

        #endregion

        #region Overrides

        #region RegionEndpointServiceName
        private const string ServiceName = "mic";

        /// <inheritdoc />
        public override string RegionEndpointServiceName => ServiceName;
        #endregion

        #region ServiceVersion
        private static readonly string serviceVersion = typeof(MicConfig)
#if NETSTANDARD1_3
            .GetTypeInfo()
#endif
            .Assembly.GetName().Version.ToString();

        /// <inheritdoc />
        public override string ServiceVersion => serviceVersion;
        #endregion

        #region UserAgent
        private static readonly string sdkVersion = typeof(ClientConfig)
#if NETSTANDARD1_3
            .GetTypeInfo()
#endif
            .Assembly.GetName().Version.ToString();

        private static readonly string userAgent = InternalSDKUtils.BuildUserAgentString(sdkVersion);

        /// <inheritdoc />
        public override string UserAgent => userAgent;
        #endregion

        #endregion

        #region Public methods
        public void PushAllMarkedProperties()
        {
            var configs = clientConfigAccessor().ToList();
            foreach (var action in pushMarkedOfTypeActions)
                action(configs);
        }

        public void UnmarkAllProperties()
        {
            for (int i = 0; i < markedProperty.Length; i++)
                markedProperty[i] = false;
        }
        #endregion

        #region Private Helpers
        private void SetProperty<T>(string name, T value, bool fromThis = false)
        {
            var info = (ClientConfigPropertyInfo<T>)propertyInfo[name];
            foreach (var config in clientConfigAccessor())
                info.Setter(config, value);
            if (!fromThis)
                info.Setter(this, value);
            markedProperty[info.Index] = true;
        }

        private void PushMarkedPropertiesOfType<T>(List<ClientConfig> configs)
        {
            foreach (var info in propertyInfo.Values.OfType<ClientConfigPropertyInfo<T>>())
            {
                if (markedProperty[info.Index])
                {
                    var value = info.Getter(this);
                    foreach (var config in configs)
                        info.Setter(config, value);
                }
            }
        }
        #endregion

        #region Statics

        private class ClientConfigPropertyInfo
        {
            public PropertyInfo Metadata { get; set; }
            public int Index { get; set; }
        }

        private class ClientConfigPropertyInfo<T> : ClientConfigPropertyInfo
        {
            public static ClientConfigPropertyInfo FromPropertyInfo(PropertyInfo pi)
            {
                var configParam = Expression.Parameter(typeof(ClientConfig), "config");
                var propertyExpr = Expression.Property(configParam, pi);
                var getterLambda = Expression.Lambda<Func<ClientConfig, T>>(propertyExpr, configParam);

                var getterFunc = getterLambda.Compile();

                var valueParam = Expression.Parameter(typeof(T), "value");
                var setterExpr = Expression.Assign(propertyExpr, valueParam);
                var setterLambda = Expression.Lambda<Action<ClientConfig, T>>(setterExpr, configParam, valueParam);
                var setterAction = setterLambda.Compile();

                return new ClientConfigPropertyInfo<T>
                {
                    Metadata = pi,
                    Getter = getterFunc,
                    Setter = setterAction
                };
            }

            public Action<ClientConfig, T> Setter { get; set; }
            public Func<ClientConfig, T> Getter { get; set; }
        }

        private static readonly Dictionary<string, ClientConfigPropertyInfo> propertyInfo;
        private static readonly List<MethodInfo> pushMarkedOfTypeInfos;

        static MicConfig()
        {
            var configProperties = typeof(ClientConfig).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(pi => pi.CanRead && pi.CanWrite).ToList();
            var openConfigPropInfoType = typeof(ClientConfigPropertyInfo<>);
            propertyInfo = configProperties.Select((pi, i) =>
            {
                var genericConfigPropInfoType = openConfigPropInfoType.MakeGenericType(pi.PropertyType);
                var fromPropertyInfoMethod = genericConfigPropInfoType.GetMethod(nameof(ClientConfigPropertyInfo<object>.FromPropertyInfo), BindingFlags.Public | BindingFlags.Static);
                var fromPropertyInfoFunc = (Func<PropertyInfo, ClientConfigPropertyInfo>)fromPropertyInfoMethod.CreateDelegate(typeof(Func<PropertyInfo, ClientConfigPropertyInfo>));
                var info = fromPropertyInfoFunc(pi);
                info.Index = i;
                return info;
            }).ToDictionary(i => i.Metadata.Name, StringComparer.Ordinal);

            var openPushMarkedPropertiesOfTypeInfo = typeof(MicConfig)
                .GetMethod(nameof(PushMarkedPropertiesOfType), BindingFlags.Instance | BindingFlags.NonPublic);
            pushMarkedOfTypeInfos = propertyInfo.Values
                .Select(info => info.Metadata.PropertyType)
                .Distinct()
                .Select(t => openPushMarkedPropertiesOfTypeInfo.MakeGenericMethod(t))
                .ToList();
        }

        private readonly List<Action<List<ClientConfig>>> pushMarkedOfTypeActions;
        private readonly bool[] markedProperty = new bool[propertyInfo.Count];

        private MicConfig() : base()
        {
            pushMarkedOfTypeActions = pushMarkedOfTypeInfos
                .Select(mi => mi.CreateDelegate(typeof(Action<List<ClientConfig>>), this))
                .Cast<Action<List<ClientConfig>>>()
                .ToList();
        }

        #endregion
    }
}
