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
using System;
using System.Security;

namespace Microsoft.Azure.Commands.ResourceManager.Automation.Test.UnitTests
{
    [TestClass]
    public class NewAzureAutomationCertificateTest : RMTestBase
    {
        private Mock<IAutomationClient> mockAutomationClient;

        private MockCommandRuntime mockCommandRuntime;

        private NewAzureAutomationCertificate cmdlet;

        [TestInitialize]
        public void SetupTest()
        {
            mockAutomationClient = new Mock<IAutomationClient>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new NewAzureAutomationCertificate
            {
                AutomationClient = mockAutomationClient.Object,
                CommandRuntime = mockCommandRuntime
            };
        }

        [TestMethod]
        public void NewAzureAutomationCertificateByNameSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            string certificateName = "certificate";
            string path = "testCert.pfx";
            string password = "password";
            string description = "desc";

            var secureString = new SecureString();
            Array.ForEach(password.ToCharArray(), secureString.AppendChar);
            secureString.MakeReadOnly();

            mockAutomationClient.Setup(
                f => f.CreateCertificate(resourceGroupName, accountName, certificateName, path, secureString, description, false));

            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.Name = certificateName;
            cmdlet.Description = description;
            cmdlet.Path = path;
            cmdlet.Password = secureString;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.CreateCertificate(resourceGroupName, accountName, certificateName, path, secureString, description, false), Times.Once());
        }
    }
}
