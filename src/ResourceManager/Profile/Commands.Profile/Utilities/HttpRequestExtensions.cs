﻿using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.Azure.Commands.Common.Authentication;
using Microsoft.Azure.Commands.Common.Authentication.Abstractions;

namespace Microsoft.Azure.Commands.Profile.Utilities
{
    internal static class HttpRequestExtensions
    {
        public static void InjectAzureAuthentication(this HttpRequestMessage request, IAzureContext context) =>
            request.InjectAzureAuthentication(context, CancellationToken.None);

        public static void InjectAzureAuthentication(this HttpRequestMessage request, IAzureContext context, CancellationToken token) => 
            AzureSession.Instance.AuthenticationFactory.GetServiceClientCredentials(context).ProcessHttpRequestAsync(request, token).Wait(token);

        public static void AddUserAgent(this HttpRequestHeaders headers)
        {
            var userAgent = AzureSession.Instance.ClientFactory.UserAgents.LastOrDefault();
            if (userAgent != null)
            {
                headers.Add("user-agent", userAgent.ToString());
            }
        }

        public static void AddRange(this HttpRequestHeaders headers, IDictionary collection)
        {
            foreach (DictionaryEntry pair in collection)
            {
                headers.Add(pair.Key.ToString(), pair.Value.ToString());
            }
        }
    }
}