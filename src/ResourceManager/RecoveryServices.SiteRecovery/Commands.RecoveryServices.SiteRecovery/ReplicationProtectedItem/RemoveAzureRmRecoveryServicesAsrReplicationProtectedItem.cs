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

using System.Management.Automation;
using Microsoft.Azure.Management.RecoveryServices.SiteRecovery.Models;
using Job = Microsoft.Azure.Management.RecoveryServices.SiteRecovery.Models.Job;

namespace Microsoft.Azure.Commands.RecoveryServices.SiteRecovery
{
    /// <summary>
    ///     Stops/Disables replication for an Azure Site Recovery replication protected item.
    /// </summary>
    [Cmdlet(
        VerbsCommon.Remove,
        "AzureRmRecoveryServicesAsrReplicationProtectedItem",
        DefaultParameterSetName = ASRParameterSets.DisableDR,
        SupportsShouldProcess = true)]
    [Alias("Remove-ASRReplicationProtectedItem")]
    [OutputType(typeof(ASRJob))]
    public class RemoveAzureRmRecoveryServicesAsrReplicationProtectedItem : SiteRecoveryCmdletBase
    {
        /// <summary>
        ///     Job Response
        /// </summary>
        private Job jobResponse;

        /// <summary>
        ///     Long Running Operation Response
        /// </summary>
        private PSSiteRecoveryLongRunningOperation response;

        /// <summary>
        ///     Gets or sets replication protected item object corresponding to the 
        ///     replication protected item for which replication is to be disabled.
        /// </summary>
        [Parameter(
            ParameterSetName = ASRParameterSets.DisableDR,
            Mandatory = true,
            ValueFromPipeline = true)]
        [ValidateNotNullOrEmpty]
        [Alias("ReplicationProtectedItem")]
        public ASRReplicationProtectedItem InputObject { get; set; }

        /// <summary>
        ///     Gets or sets switch parameter. On passing, command waits till completion.
        /// </summary>
        [Parameter]
        public SwitchParameter WaitForCompletion { get; set; }

        /// <summary>
        ///     Gets or sets switch parameter. On passing, command does not ask for confirmation.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        /// <summary>
        ///     ProcessRecord of the command.
        /// </summary>
        public override void ExecuteSiteRecoveryCmdlet()
        {
            base.ExecuteSiteRecoveryCmdlet();

            if (ShouldProcess(
                InputObject.FriendlyName,
                VerbsCommon.Remove))
            {

                if (!Force.IsPresent)
                {
                    var input = new DisableProtectionInput();
                    input.Properties = new DisableProtectionInputProperties
                    {
                        ReplicationProviderInput = new DisableProtectionProviderSpecificInput()
                    };
                    response = RecoveryServicesClient.DisableProtection(
                        Utilities.GetValueFromArmId(
                            InputObject.ID,
                            ARMResourceTypeConstants.ReplicationFabrics),
                        Utilities.GetValueFromArmId(
                            InputObject.ID,
                            ARMResourceTypeConstants.ReplicationProtectionContainers),
                        InputObject.Name,
                        input);
                }
                else
                {
                    response = RecoveryServicesClient.PurgeProtection(
                        Utilities.GetValueFromArmId(
                            InputObject.ID,
                            ARMResourceTypeConstants.ReplicationFabrics),
                        Utilities.GetValueFromArmId(
                            InputObject.ID,
                            ARMResourceTypeConstants.ReplicationProtectionContainers),
                        InputObject.Name);
                }

                jobResponse = RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(
                    PSRecoveryServicesClient.GetJobIdFromReponseLocation(response.Location));

                WriteObject(new ASRJob(jobResponse));

                if (WaitForCompletion.IsPresent)
                {
                    WaitForJobCompletion(jobResponse.Name);

                    jobResponse = RecoveryServicesClient.GetAzureSiteRecoveryJobDetails(
                        PSRecoveryServicesClient
                            .GetJobIdFromReponseLocation(response.Location));

                    WriteObject(new ASRJob(jobResponse));
                }
            }
        }

        /// <summary>
        ///     Writes Job.
        /// </summary>
        /// <param name="job">JOB object</param>
        private void WriteJob(
            Job job)
        {
            WriteObject(new ASRJob(job));
        }
    }
}