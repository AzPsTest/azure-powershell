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

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Azure.Commands.Common
{

  using GetEventData = Func<EventArgs>;
  using SignalDelegate = Func<string, CancellationToken, Func<EventArgs>, Task>;

  internal static class EventHelper
    {
        public static EventData CreateLogEvent(string message)
        {
            return new EventData
            {
                Id = Guid.NewGuid().ToString(),
                Message = message

            };
        }

        public static async void Print( this GetEventData getEventData, SignalDelegate signal, CancellationToken token, string streamName, string eventName)
        {
            var eventDisplayName = SplitPascalCase(eventName).ToUpperInvariant();
            var data = EventDataConverter.ConvertFrom(getEventData()); // also, we manually use our TypeConverter to return an appropriate type
            if (data.Id != "Verbose" && data.Id != "Warning" && data.Id != "Debug" && data.Id != "Information" && data.Id != "Error")
            {
                await signal(streamName, token, () => EventHelper.CreateLogEvent($"{eventDisplayName} The contents are '{data?.Id}' and '{data?.Message}'"));
                if (data != null)
                {
                    await signal(streamName, token, () => EventHelper.CreateLogEvent($"{eventDisplayName} Parameter: '{data.Parameter}'\n{eventDisplayName} RequestMessage '{data.RequestMessage}'\n{eventDisplayName} Response: '{data.ResponseMessage}'\n{eventDisplayName} Value: '{data.Value}'"));
                    await signal(streamName, token, () => EventHelper.CreateLogEvent($"{eventDisplayName} ExtendedData Type: '{data.ExtendedData?.GetType()}'\n{eventDisplayName} ExtendedData '{data.ExtendedData}'"));
                }
            }
        }

        static string SplitPascalCase(string word)
        {
            var regex = new Regex("([a-z]+)([A-Z])");
            var output = regex.Replace(word, "$1 $2");
            regex = new Regex("([A-Z])([A-Z][a-z])");
            return regex.Replace(output, "$1 $2");
        }
    }

}
