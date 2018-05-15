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

using Microsoft.Azure.Management.Monitor.Management.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.Insights.OutputClasses
{
    /// <summary>
    /// Wrapps around the ServiceDiagnosticSettings
    /// </summary>
    public class PSServiceDiagnosticSettings : ServiceDiagnosticSettingsResource
    {
        /// <summary>
        /// Initializes a new instance of the PSServiceDiagnosticSettings class.
        /// </summary>
        public PSServiceDiagnosticSettings(ServiceDiagnosticSettingsResource serviceDiagnosticSettings)
            : base(
                name: serviceDiagnosticSettings.Name,
                id: serviceDiagnosticSettings.Id, 
                location: serviceDiagnosticSettings.Location, 
                type: serviceDiagnosticSettings.Type,
                metrics: serviceDiagnosticSettings.Metrics, 
                logs: serviceDiagnosticSettings.Logs)
        {
            StorageAccountId = serviceDiagnosticSettings.StorageAccountId;
            ServiceBusRuleId = serviceDiagnosticSettings.ServiceBusRuleId;
            EventHubAuthorizationRuleId = serviceDiagnosticSettings.EventHubAuthorizationRuleId;
            Metrics = new List<MetricSettings>();
            foreach (MetricSettings metricSettings in serviceDiagnosticSettings.Metrics)
            {
                Metrics.Add(new PSMetricSettings(metricSettings));
            }

            Logs = new List<LogSettings>();
            foreach (LogSettings logSettings in serviceDiagnosticSettings.Logs)
            {
                Logs.Add(new PSLogSettings(logSettings));
            }

            WorkspaceId = serviceDiagnosticSettings.WorkspaceId;
            Tags = serviceDiagnosticSettings.Tags;
        }
    }
}
