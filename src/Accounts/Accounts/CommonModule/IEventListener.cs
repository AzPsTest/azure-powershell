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
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GetEventData = System.Func<Microsoft.Azure.Commands.Common.EventData>;


namespace Microsoft.Azure.Commands.Common
{
  /// <summary>
  /// The IEventListener Interface defines the communication mechanism for Signaling events during a remote call.
  /// </summary>
  /// <remarks>
  /// The interface is designed to be as minimal as possible, allow for quick peeking of the event type (<c>id</c>) 
  /// and the cancellation status and provides a delegate for retrieving the event details themselves.
  /// </remarks>
  public interface IEventListener
    {
        Task Signal(string id, CancellationToken token, GetEventData createMessage);
        CancellationToken Token { get; }
        System.Action Cancel { get; }
    }

}