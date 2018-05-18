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

using Microsoft.Azure.Commands.Sql.Location_Capabilities.Cmdlet;
using Microsoft.Azure.Commands.Sql.Test.Utilities;
using Microsoft.Azure.ServiceManagemenet.Common.Models;
using Microsoft.WindowsAzure.Commands.ScenarioTest;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Azure.Commands.Sql.Test.UnitTests
{
    public class AzureSqlCapabilityAttributeTests
    {
        public AzureSqlCapabilityAttributeTests(ITestOutputHelper output)
        {
            XunitTracingInterceptor.AddToContext(new XunitTracingInterceptor(output));
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void GetAzureSqlCapabilityAttributes()
        {
            Type type = typeof(GetAzureSqlCapability);
            UnitTestHelper.CheckCmdletModifiesData(type, false);
            UnitTestHelper.CheckConfirmImpact(type, System.Management.Automation.ConfirmImpact.None);

            UnitTestHelper.CheckCmdletParameterAttributes(type, "LocationName", true, true);
            UnitTestHelper.CheckCmdletParameterAttributes(type, "ServerVersionName", false, true);
            UnitTestHelper.CheckCmdletParameterAttributes(type, "EditionName", false, true);
            UnitTestHelper.CheckCmdletParameterAttributes(type, "ServiceObjectiveName", false, true);
            UnitTestHelper.CheckCmdletParameterAttributes(type, "Defaults", false, false);
        }
    }
}
