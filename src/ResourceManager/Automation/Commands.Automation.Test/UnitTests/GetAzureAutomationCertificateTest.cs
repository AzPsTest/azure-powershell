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
using Microsoft.Azure.Commands.Automation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Commands.Common.Test.Mocks;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Moq;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.ResourceManager.Automation.Test.UnitTests
{
    [TestClass]
    public class GetAzureAutomationCertificateTest : RMTestBase
    {
        private Mock<IAutomationClient> mockAutomationClient;

        private MockCommandRuntime mockCommandRuntime;

        private GetAzureAutomationCertificate cmdlet;

        [TestInitialize]
        public void SetupTest()
        {
            mockAutomationClient = new Mock<IAutomationClient>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new GetAzureAutomationCertificate
            {
                AutomationClient = mockAutomationClient.Object,
                CommandRuntime = mockCommandRuntime
            };
        }

        [TestMethod]
        public void GetAzureAutomationCertificateByNameSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            string certificateName = "certificate";

            mockAutomationClient.Setup(f => f.GetCertificate(resourceGroupName, accountName, certificateName));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.Name = certificateName;
            cmdlet.SetParameterSet("ByCertificateName");
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.GetCertificate(resourceGroupName, accountName, certificateName), Times.Once());
        }

        [TestMethod]
        public void GetAzureAutomationCertificateByAllSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            string nextLink = string.Empty;

            mockAutomationClient.Setup(f => f.ListCertificates(resourceGroupName, accountName, ref nextLink)).Returns((string a, string b, string c) => new List<CertificateInfo>());

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.ListCertificates(resourceGroupName, accountName, ref nextLink), Times.Once());
        }
    }
}
