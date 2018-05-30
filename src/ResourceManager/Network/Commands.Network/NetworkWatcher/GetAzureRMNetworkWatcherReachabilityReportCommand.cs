// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Management.Network.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AutoMapper;
using CNM = Microsoft.Azure.Commands.Network.Models;
using MNM = Microsoft.Azure.Management.Network.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;

namespace Microsoft.Azure.Commands.Network.Automation
{
    [Cmdlet(VerbsCommon.Get, "AzureRMNetworkWatcherReachabilityReport", DefaultParameterSetName = "SetByName"), OutputType(typeof(PSAzureReachabilityReport))]
    public partial class GetAzureRMNetworkWatcherReachabilityReport : NetworkWatcherBaseCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            HelpMessage = "The network watcher resource",
            ParameterSetName = "SetByResource")]
        public PSNetworkWatcher NetworkWatcher { get; set; }

        [Alias("ResourceName", "Name")]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The name of network watcher.",
            ParameterSetName = "SetByName")]
        [ValidateNotNullOrEmpty]
        public string NetworkWatcherName { get; set; }


        [Parameter(
            Mandatory = true,
            HelpMessage = "The name of the network watcher resource group.",
            ParameterSetName = "SetByName")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The Id of network watcher resource.",
            ParameterSetName = "SetByResourceId")]
        public string ResourceId { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "Location of the network watcher.",
            ParameterSetName = "SetByLocation")]
        [LocationCompleter("Microsoft.Network/networkWatchers")]
        [ValidateNotNull]
        public string NetworkWatcherLocation { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "List of Internet service providers.")]
        public List<string> Provider { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Optional Azure regions to scope the query to.")]
        [LocationCompleter("Microsoft.Network/networkWatchers")]
        public List<string> Location { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The start time for the Azure reachability report.")]
        public DateTime StartTime { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The end time for the Azure reachability report.")]
        public DateTime EndTime { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The name of the country.")]
        public string Country { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The name of the state.")]
        public string State { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "The name of the city.")]
        public string City { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background")]
        public SwitchParameter AsJob { get; set; }

        public override void Execute()
        {
            base.Execute();

            // ProviderLocation
            PSAzureReachabilityReportLocation vProviderLocation = null;

            if (Country != null)
            {
                if (vProviderLocation == null)
                {
                    vProviderLocation = new PSAzureReachabilityReportLocation();
                }
                vProviderLocation.Country = Country;
            }

            if (State != null)
            {
                if (vProviderLocation == null)
                {
                    vProviderLocation = new PSAzureReachabilityReportLocation();
                }
                vProviderLocation.State = State;
            }

            if (City != null)
            {
                if (vProviderLocation == null)
                {
                    vProviderLocation = new PSAzureReachabilityReportLocation();
                }
                vProviderLocation.City = City;
            }

            var vAzureReachabilityReportParameters = new AzureReachabilityReportParameters
            {
                Providers = Provider,
                AzureLocations = Location,
                StartTime = StartTime,
                EndTime = EndTime,
                ProviderLocation = NetworkResourceManagerProfile.Mapper.Map < AzureReachabilityReportLocation >(vProviderLocation),
            };

            if (string.Equals(ParameterSetName, "SetByLocation", StringComparison.OrdinalIgnoreCase))
            {
                var networkWatcher = GetNetworkWatcherByLocation(NetworkWatcherLocation);

                if (networkWatcher == null)
                {
                    throw new ArgumentException("There is no network watcher in location {0}", NetworkWatcherLocation);
                }

                ResourceGroupName = NetworkBaseCmdlet.GetResourceGroup(networkWatcher.Id);
                NetworkWatcherName = networkWatcher.Name;
            }

            if (string.Equals(ParameterSetName, "SetByResource", StringComparison.OrdinalIgnoreCase))
            {
                ResourceGroupName = NetworkWatcher.ResourceGroupName;
                NetworkWatcherName = NetworkWatcher.Name;
            }

            if (string.Equals(ParameterSetName, "SetByResourceId", StringComparison.OrdinalIgnoreCase))
            {
                var resourceInfo = new ResourceIdentifier(ResourceId);
                ResourceGroupName = resourceInfo.ResourceGroupName;
                NetworkWatcherName = resourceInfo.ResourceName;
            }

            var vNetworkWatcherResult = NetworkClient.NetworkManagementClient.NetworkWatchers.GetAzureReachabilityReport(ResourceGroupName, NetworkWatcherName, vAzureReachabilityReportParameters);
            var vNetworkWatcherModel = NetworkResourceManagerProfile.Mapper.Map<PSAzureReachabilityReport>(vNetworkWatcherResult);
            WriteObject(vNetworkWatcherModel);
        }
    }
}
