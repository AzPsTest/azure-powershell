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

using Microsoft.Azure.Portal.RecoveryServices.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Commands.RecoveryServices.lib
{
    public static class Utilities
    {
        /// <summary>
        /// Serialize the T as xml using DataContractSerializer
        /// </summary>
        /// <typeparam name="T">the type name</typeparam>
        /// <param name="value">the T object.</param>
        /// <returns>the serialized object.</returns>
        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            string serializedValue;

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memoryStream, value);
                memoryStream.Position = 0;
                serializedValue = reader.ReadToEnd();
            }

            return serializedValue;
        }


        /// <summary>
        /// Deserialize the xml as T
        /// </summary>
        /// <typeparam name="T">the type name</typeparam>
        /// <param name="xml">the xml as string</param>
        /// <returns>the eqvivalant T</returns>
        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(typeof(T));
                return (T)deserializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Method to write content to a file.
        /// </summary>
        /// <param name="fileContent">content to be written to the file</param>
        /// <param name="filePath">the path where the file is to be created</param>
        /// <param name="fileName">name of the file to be created</param>
        public static void WriteToFile<T>(T fileContent, string filePath, string fileName)
        {
            string fullFileName = Path.Combine(filePath, fileName);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@fullFileName, true))
            {
                string contentToWrite = Serialize<T>(fileContent);
                file.WriteLine(contentToWrite);
            }
        }

        /// <summary>
        /// Imports Azure Site Recovery Vault settings.
        /// </summary>
        /// <param name="asrVaultCreds">ASR Vault credentials</param>
        public static void UpdateVaultSettings(ASRVaultCreds asrVaultCreds)
        {
            object updateVaultSettingsOneAtATime = new object();
            lock (updateVaultSettingsOneAtATime)
            {
                PSRecoveryServicesClient.asrVaultCreds.ResourceName =
                    asrVaultCreds.ResourceName;
                PSRecoveryServicesClient.asrVaultCreds.CloudServiceName =
                    asrVaultCreds.CloudServiceName;
                PSRecoveryServicesClient.asrVaultCreds.ChannelIntegrityKey =
                    asrVaultCreds.ChannelIntegrityKey;
            }
        }

        /// <summary>
        /// method to return the Cownloads path for the curretn user.
        /// </summary>
        /// <returns>path as  string.</returns>
        public static string GetDefaultPath()
        {
            string path = null;
            path = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            path = Path.Combine(path, "Downloads");

            return path;
        }
    }
}
