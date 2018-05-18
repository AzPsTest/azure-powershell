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

using Microsoft.Azure.Commands.Automation.Cmdlet;
using Microsoft.Azure.Commands.Automation.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Commands.Common.Test.Mocks;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Moq;
using System;
namespace Microsoft.Azure.Commands.ResourceManager.Automation.Test.UnitTests
{
    [TestClass]
    public class UnregisterAzureAutomationScheduledRunbookTest : RMTestBase
    {
        private Mock<IAutomationClient> mockAutomationClient;

        private MockCommandRuntime mockCommandRuntime;

        private UnregisterAzureAutomationScheduledRunbook cmdlet;

        [TestInitialize]
        public void SetupTest()
        {
            mockAutomationClient = new Mock<IAutomationClient>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new UnregisterAzureAutomationScheduledRunbook
            {
                AutomationClient = mockAutomationClient.Object,
                CommandRuntime = mockCommandRuntime
            };
        }

        [TestMethod]
        public void UnregisterAzureAutomationScheduledRunbookByIdSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            var jobScheduleId = new Guid();

            mockAutomationClient.Setup(f => f.UnregisterScheduledRunbook(resourceGroupName, accountName, jobScheduleId));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.JobScheduleId = jobScheduleId;
            cmdlet.SetParameterSet(AutomationCmdletParameterSets.ByJobScheduleId);
            cmdlet.Force = true;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.UnregisterScheduledRunbook(resourceGroupName, accountName, jobScheduleId), Times.Once());
        }

        [TestMethod]
        public void UnregisterAzureAutomationScheduledRunbookByRunbookNameAndScheduleNameSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            string runbookName = "runbook";
            string scheduleName = "schedule";

            mockAutomationClient.Setup(f => f.UnregisterScheduledRunbook(resourceGroupName, accountName, runbookName, scheduleName));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.RunbookName = runbookName;
            cmdlet.ScheduleName = scheduleName;
            cmdlet.SetParameterSet(AutomationCmdletParameterSets.ByRunbookNameAndScheduleName);
            cmdlet.Force = true;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.UnregisterScheduledRunbook(resourceGroupName, accountName, runbookName, scheduleName), Times.Once());
        }
    }
}
