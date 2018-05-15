// ----------------------------------------------------------------------------------
// 
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Hyak.Common;
using Microsoft.Azure.Commands.RecoveryServices.SiteRecovery.Properties;
using Microsoft.Azure.Management.RecoveryServices.SiteRecovery.Models;

namespace Microsoft.Azure.Commands.RecoveryServices.SiteRecovery
{
    /// <summary>
    ///     Get the details of an Azure Site Recovery Fabric.
    /// </summary>
    [Cmdlet(
        VerbsCommon.Get,
        "AzureRmRecoveryServicesAsrFabric",
        DefaultParameterSetName = ASRParameterSets.Default)]
    [Alias("Get-ASRFabric")]
    [OutputType(typeof(List<ASRFabric>))]
    public class GetAzureRmRecoveryServicesAsrFabric : SiteRecoveryCmdletBase
    {
        /// <summary>
        ///     Gets or sets the name of the fabric to look for.
        /// </summary>
        [Parameter(
            ParameterSetName = ASRParameterSets.ByName,
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the friendly name of the fabric to look for.
        /// </summary>
        [Parameter(
            ParameterSetName = ASRParameterSets.ByFriendlyName,
            Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string FriendlyName { get; set; }

        /// <summary>
        ///     ProcessRecord of the command.
        /// </summary>
        public override void ExecuteSiteRecoveryCmdlet()
        {
            base.ExecuteSiteRecoveryCmdlet();

            switch (ParameterSetName)
            {
                case ASRParameterSets.ByName:
                    GetByName();
                    break;
                case ASRParameterSets.ByFriendlyName:
                    GetByFriendlyName();
                    break;
                case ASRParameterSets.Default:
                    GetAll();
                    break;
            }
        }

        /// <summary>
        ///     Queries all / by default.
        /// </summary>
        private void GetAll()
        {
            var fabricListResponse = RecoveryServicesClient.GetAzureSiteRecoveryFabric();

            foreach (var fabric in fabricListResponse)
            {
                WriteFabric(fabric);
            }
        }

        /// <summary>
        ///     Queries by friendly name.
        /// </summary>
        private void GetByFriendlyName()
        {
            var fabricListResponse = RecoveryServicesClient.GetAzureSiteRecoveryFabric();

            var found = false;
            foreach (var fabric in fabricListResponse)
            {
                if (0 ==
                    string.Compare(
                        FriendlyName,
                        fabric.Properties.FriendlyName,
                        StringComparison.OrdinalIgnoreCase))
                {
                    var fabricByName =
                        RecoveryServicesClient.GetAzureSiteRecoveryFabric(fabric.Name);
                    WriteFabric(fabricByName);

                    found = true;
                }
            }

            if (!found)
            {
                throw new InvalidOperationException(
                    string.Format(
                        Resources.FabricNotFound,
                        FriendlyName,
                        PSRecoveryServicesClient.asrVaultCreds.ResourceName));
            }
        }

        /// <summary>
        ///     Queries by name.
        /// </summary>
        private void GetByName()
        {
            try
            {
                var fabricResponse =
                    RecoveryServicesClient.GetAzureSiteRecoveryFabric(Name);

                if (fabricResponse != null)
                {
                    WriteFabric(fabricResponse);
                }
            }
            catch (CloudException ex)
            {
                if (string.Compare(
                        ex.Error.Code,
                        "NotFound",
                        StringComparison.OrdinalIgnoreCase) ==
                    0)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            Resources.FabricNotFound,
                            Name,
                            PSRecoveryServicesClient.asrVaultCreds.ResourceName));
                }

                throw;
            }
        }

        /// <summary>
        ///     Write Powershell Fabric.
        /// </summary>
        /// <param name="server">Fabric object</param>
        private void WriteFabric(
            Fabric fabric)
        {
            WriteObject(new ASRFabric(fabric));
        }
    }
}