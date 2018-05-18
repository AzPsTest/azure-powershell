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
using System.Management.Automation;
using System.Security;

namespace Microsoft.Azure.Commands.ResourceManager.Automation.Test.UnitTests
{
    [TestClass]
    public class SetAzureAutomationCredentialTest : RMTestBase
    {
        private Mock<IAutomationClient> mockAutomationClient;

        private MockCommandRuntime mockCommandRuntime;

        private SetAzureAutomationCredential cmdlet;

        [TestInitialize]
        public void SetupTest()
        {
            mockAutomationClient = new Mock<IAutomationClient>();
            mockCommandRuntime = new MockCommandRuntime();
            cmdlet = new SetAzureAutomationCredential
            {
                AutomationClient = mockAutomationClient.Object,
                CommandRuntime = mockCommandRuntime
            };
        }

        [TestMethod]
        public void SetAzureAutomationCredentialByNameSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            string credentialName = "credential";

            mockAutomationClient.Setup(f => f.UpdateCredential(resourceGroupName, accountName, credentialName, null, null, null));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.Name = credentialName;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.UpdateCredential(resourceGroupName, accountName, credentialName, null, null, null), Times.Once());
        }

        [TestMethod]
        public void SetAzureAutomationCredentialByNameWithParametersSuccessfull()
        {
            // Setup
            string resourceGroupName = "resourceGroup";
            string accountName = "automation";
            string credentialName = "credential";
            string username = "testUser";
            string password = "password";
            string description = "desc";

            var secureString = new SecureString();
            Array.ForEach(password.ToCharArray(), secureString.AppendChar);
            secureString.MakeReadOnly();

            var value = new PSCredential(username, secureString);

            mockAutomationClient.Setup(f => f.UpdateCredential(resourceGroupName, accountName, credentialName, username, password, description));

            // Test
            cmdlet.ResourceGroupName = resourceGroupName;
            cmdlet.AutomationAccountName = accountName;
            cmdlet.Name = credentialName;
            cmdlet.Description = description;
            cmdlet.Value = value;
            cmdlet.ExecuteCmdlet();

            // Assert
            mockAutomationClient.Verify(f => f.UpdateCredential(resourceGroupName, accountName, credentialName, username, password, description), Times.Once());
        }
    }
}
