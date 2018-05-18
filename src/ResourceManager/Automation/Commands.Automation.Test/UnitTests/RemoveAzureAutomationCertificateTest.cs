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
using Moq;

namespace Microsoft.Azure.Commands.ResourceManager.Automation.Test.UnitTests
{
    [TestClass]
    public class RemoveAzureAutomationCertificateTest : RMTestBase
    {
        private Mock<IAutomationClient> mockAutomationClient;

        private MockCommandRuntime mockCommandRuntime;

        private RemoveAzureAutomationCertificate cmdlet;

        [TestInitialize]
        public void SetupTest()
        {
            mockAutomationClient = new Mock<IAutomationClient>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new RemoveAzureAutomationCertificate
            {
                AutomationClient = mockAutomationClient.Object,
                CommandRuntime = mockCommandRuntime
            };
        }

        [TestMethod]
        public void RemoveAzureAutomationCertificateByNameSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            string certificateName = "cert";

            mockAutomationClient.Setup(f => f.DeleteCertificate(resourceGroupName, accountName, certificateName));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.Name = certificateName;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.DeleteCertificate(resourceGroupName, accountName, certificateName), Times.Once());
        }
    }
}
