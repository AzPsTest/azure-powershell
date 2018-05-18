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

using System;
using Microsoft.Azure.Commands.Automation.Cmdlet;
using Microsoft.Azure.Commands.Automation.Common;
using Microsoft.Azure.Commands.Automation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Commands.Common.Test.Mocks;
using Microsoft.WindowsAzure.Commands.ScenarioTest;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Microsoft.Azure.Commands.ResourceManager.Automation.Test.UnitTests
{
    public class GetAzureAutomationDscCompilationJobTest : RMTestBase
    {
        private Mock<IAutomationClient> mockAutomationClient;

        private MockCommandRuntime mockCommandRuntime;

        private GetAzureAutomationDscCompilationJob cmdlet;

        
        public GetAzureAutomationDscCompilationJobTest()
        {
            mockAutomationClient = new Mock<IAutomationClient>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new GetAzureAutomationDscCompilationJob
            {
                AutomationClient = mockAutomationClient.Object,
                CommandRuntime = mockCommandRuntime
            };
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]

        public void GetAzureAutomationGetCompilationJobByIdSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "account";
            Guid id = Guid.NewGuid();
            
            mockAutomationClient.Setup(f => f.GetCompilationJob(resourceGroupName, accountName, id)).Returns((string a, string b, Guid c) => new CompilationJob());

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.Id = id;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.GetCompilationJob(resourceGroupName, accountName, id), Times.Once());
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]

        public void GetAzureAutomationCompilationJobsByConfigurationNameSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "account";
            string configurationName = "configuration";
            string nextLink = string.Empty;
            DateTimeOffset startTime = DateTimeOffset.Now;
            DateTimeOffset endTime = DateTimeOffset.Now;
            string status = "Completed";

            mockAutomationClient.Setup(f => f.ListCompilationJobsByConfigurationName(resourceGroupName, accountName, configurationName, startTime, endTime, status, ref nextLink));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.ConfigurationName = configurationName;
            cmdlet.StartTime = startTime;
            cmdlet.EndTime = endTime;
            cmdlet.Status = status;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.ListCompilationJobsByConfigurationName(resourceGroupName, accountName, configurationName, startTime, endTime, status, ref nextLink), Times.Once());
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]

        public void GetAzureAutomationListCompilationJobs()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "account";
            string nextLink = string.Empty;
            DateTimeOffset startTime = DateTimeOffset.Now;
            DateTimeOffset endTime = DateTimeOffset.Now;
            string status = "Completed";

            mockAutomationClient.Setup(f => f.ListCompilationJobs(resourceGroupName, accountName, startTime, endTime, status, ref nextLink));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.StartTime = startTime;
            cmdlet.EndTime = endTime;
            cmdlet.Status = status;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.ListCompilationJobs(resourceGroupName, accountName, startTime, endTime, status, ref nextLink), Times.Once());
        }
    }
}