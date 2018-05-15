﻿// ----------------------------------------------------------------------------------
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

using Microsoft.Azure.Commands.Automation.Common;
using Microsoft.Azure.Commands.Automation.Model;
using System.Collections.Generic;
using System.Management.Automation;
using System.Security.Permissions;

namespace Microsoft.Azure.Commands.Automation.Cmdlet
{
    /// <summary>
    /// Get Webhook for automation.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureRmAutomationWebhook", DefaultParameterSetName = AutomationCmdletParameterSets.ByAll)]
    [OutputType(typeof(Webhook))]
    public class GetAzureAutomationWebhook : AzureAutomationBaseCmdlet
    {
        /// <summary>
        /// Gets or sets the Webhook name.
        /// </summary>
        [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByName, Mandatory = true, ValueFromPipelineByPropertyName = true,
            HelpMessage = "The Webhook name.")]
        [Alias("WebhookName")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Webhook name.
        /// </summary>
        [Parameter(ParameterSetName = AutomationCmdletParameterSets.ByRunbookName, Mandatory = true, ValueFromPipelineByPropertyName = true,
            HelpMessage = "The Runbook name.")]
        public string RunbookName { get; set; }

        /// <summary>
        /// Execute this cmdlet.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void AutomationProcessRecord()
        {
            IEnumerable<Webhook> webhooks = null;
            if (ParameterSetName == AutomationCmdletParameterSets.ByAll)
            {
                var nextLink = string.Empty;
                do
                {
                    webhooks = AutomationClient.ListWebhooks(ResourceGroupName, AutomationAccountName, null, ref nextLink);
                    GenerateCmdletOutput(webhooks);
                }
                while (!string.IsNullOrEmpty(nextLink));

            }
            else if (ParameterSetName == AutomationCmdletParameterSets.ByName)
            {
                webhooks = new List<Webhook>
                               {
                                   AutomationClient.GetWebhook(
                                       ResourceGroupName,
                                       AutomationAccountName,
                                       Name)
                               };
                GenerateCmdletOutput(webhooks);
            }
            else if (ParameterSetName == AutomationCmdletParameterSets.ByRunbookName)
            {
                var nextLink = string.Empty;
                do
                {
                    webhooks = AutomationClient.ListWebhooks(ResourceGroupName, AutomationAccountName, RunbookName, ref nextLink);
                    GenerateCmdletOutput(webhooks);
                }
                while (!string.IsNullOrEmpty(nextLink));
            }
        }
    }
}
