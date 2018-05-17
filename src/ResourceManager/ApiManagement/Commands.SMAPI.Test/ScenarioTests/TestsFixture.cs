﻿using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;

namespace Microsoft.Azure.Commands.ApiManagement.ServiceManagement.Test.ScenarioTests
{
    using Management.ApiManagement;
    using Management.ApiManagement.Models;
    using Azure.Test;
    using System;
    using System.Linq;
    using System.Net;
    using Azure.Test.HttpRecorder;
    using WindowsAzure.Commands.Test.Utilities.Common;

    public class TestsFixture : RMTestBase
    {
        public TestsFixture()
        {
            // Initialize has bug which causes null reference exception
            HttpMockServer.FileSystemUtilsObject = new FileSystemUtils();
            TestUtilities.StartTest();
            try
            {
                var resourceManagementClient = ApiManagementHelper.GetResourceManagementClient();
                resourceManagementClient.TryRegisterSubscriptionForResource();
            }
            finally
            {
                TestUtilities.EndTest();
            }
        }
    }

    public static class ApiManagementHelper
    {
        public static ApiManagementClient GetApiManagementClient()
        {
            return TestBase.GetServiceClient<ApiManagementClient>(new CSMTestEnvironmentFactory());
        }

        public static ResourceManagementClient GetResourceManagementClient()
        {
            return TestBase.GetServiceClient<ResourceManagementClient>(new CSMTestEnvironmentFactory());
        }

        private static void ThrowIfTrue(bool condition, string message)
        {
            if (condition)
            {
                throw new Exception(message);
            }
        }

        public static void TryRegisterSubscriptionForResource(this ResourceManagementClient resourceManagementClient, string providerName = "Microsoft.ApiManagement")
        {
            var reg = resourceManagementClient.Providers.Register(providerName);
            ThrowIfTrue(reg == null, "_client.Providers.Register returned null.");
            ThrowIfTrue(reg.StatusCode != HttpStatusCode.OK, $"_client.Providers.Register returned with status code {reg.StatusCode}");

            var resultAfterRegister = resourceManagementClient.Providers.Get(providerName);
            ThrowIfTrue(resultAfterRegister == null, "_client.Providers.Get returned null.");
            ThrowIfTrue(string.IsNullOrEmpty(resultAfterRegister.Provider.Id), "Provider.Id is null or empty.");
            ThrowIfTrue(!providerName.Equals(resultAfterRegister.Provider.Namespace), $"Provider name is not equal to {providerName}.");
            ThrowIfTrue(ProviderRegistrationState.Registered != resultAfterRegister.Provider.RegistrationState &&
                        ProviderRegistrationState.Registering != resultAfterRegister.Provider.RegistrationState,
                $"Provider registration state was not 'Registered' or 'Registering', instead it was '{resultAfterRegister.Provider.RegistrationState}'");
            ThrowIfTrue(resultAfterRegister.Provider.ResourceTypes == null || resultAfterRegister.Provider.ResourceTypes.Count == 0, "Provider.ResourceTypes is empty.");
            ThrowIfTrue(resultAfterRegister.Provider.ResourceTypes[0].Locations == null || resultAfterRegister.Provider.ResourceTypes[0].Locations.Count == 0, "Provider.ResourceTypes[0].Locations is empty.");
        }

        public static string TryGetResourceGroup(this ResourceManagementClient resourceManagementClient, string location)
        {
            var resourceGroup =
                resourceManagementClient.ResourceGroups
                    .List(new ResourceGroupListParameters()).ResourceGroups
                    .Where(group => string.IsNullOrWhiteSpace(location) || group.Location.Equals(location, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault(group => group.Name.Contains("Api-Default"));

            return resourceGroup != null
                ? resourceGroup.Name
                : string.Empty;
        }

        public static void TryRegisterResourceGroup(this ResourceManagementClient resourceManagementClient, string location, string resourceGroupName)
        {
            resourceManagementClient.ResourceGroups.CreateOrUpdate(resourceGroupName, new ResourceGroup(location));
        }

        public static void TryCreateApiService(
            this ApiManagementClient client,
            string resourceGroupName,
            string apiServiceName,
            string location,
            SkuType skuType = SkuType.Developer)
        {
            client.ResourceProvider.CreateOrUpdate(
                resourceGroupName,
                apiServiceName,
                new ApiServiceCreateOrUpdateParameters
                {
                    Location = location,
                    Properties = new ApiServiceProperties
                    {
                        AddresserEmail = "foo@live.com",
                        PublisherEmail = "foo@live.com",
                        PublisherName = "apimgmt"
                    },
                    SkuProperties = new ApiServiceSkuProperties
                    {
                        Capacity = 1,
                        SkuType = skuType
                    },
                });

            var response = client.ResourceProvider.Get(resourceGroupName, apiServiceName);
            ThrowIfTrue(!response.Value.Name.Equals(apiServiceName), $"ApiService name is not equal to {apiServiceName}");
        }
    }
}