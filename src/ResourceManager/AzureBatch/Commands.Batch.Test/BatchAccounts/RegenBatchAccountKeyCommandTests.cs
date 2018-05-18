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

using Microsoft.Azure.Management.Batch.Models;
using Microsoft.WindowsAzure.Commands.ScenarioTest;
using Moq;
using System.Management.Automation;
using Xunit;
using BatchClient = Microsoft.Azure.Commands.Batch.Models.BatchClient;

namespace Microsoft.Azure.Commands.Batch.Test.Accounts
{
    public class RegenBatchAccountKeyCommandTests : WindowsAzure.Commands.Test.Utilities.Common.RMTestBase
    {
        private RegenBatchAccountKeyCommand cmdlet;
        private Mock<BatchClient> batchClientMock;
        private Mock<ICommandRuntime> commandRuntimeMock;

        public RegenBatchAccountKeyCommandTests(Xunit.Abstractions.ITestOutputHelper output)
        {
            ServiceManagemenet.Common.Models.XunitTracingInterceptor.AddToContext(new ServiceManagemenet.Common.Models.XunitTracingInterceptor(output));
            batchClientMock = new Mock<BatchClient>();
            commandRuntimeMock = new Mock<ICommandRuntime>();
            cmdlet = new RegenBatchAccountKeyCommand
            {
                CommandRuntime = commandRuntimeMock.Object,
                BatchClient = batchClientMock.Object
            };
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void RegenBatchAccountKeysTest()
        {
            string newPrimaryKey = "newPrimaryKey";
            string newSecondaryKey = "newSecondaryKey";

            string accountName = "account01";
            string resourceGroup = "resourceGroup";
            AccountKeyType keyType = AccountKeyType.Primary;
            BatchAccount accountResource = BatchTestHelpers.CreateAccountResource(accountName, resourceGroup);
            BatchAccountContext expected = BatchAccountContext.ConvertAccountResourceToNewAccountContext(accountResource, null);
            expected.PrimaryAccountKey = newPrimaryKey;
            expected.SecondaryAccountKey = newSecondaryKey;

            batchClientMock.Setup(b => b.RegenerateKeys(resourceGroup, accountName, keyType)).Returns(expected);

            cmdlet.AccountName = accountName;
            cmdlet.ResourceGroupName = resourceGroup;
            cmdlet.KeyType = keyType.ToString();
            cmdlet.ExecuteCmdlet();

            commandRuntimeMock.Verify(r => r.WriteObject(expected), Times.Once());
        }
    }
}
