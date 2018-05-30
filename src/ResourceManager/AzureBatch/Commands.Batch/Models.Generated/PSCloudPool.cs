﻿// -----------------------------------------------------------------------------
﻿//
﻿// Copyright Microsoft Corporation
﻿// Licensed under the Apache License, Version 2.0 (the "License");
﻿// you may not use this file except in compliance with the License.
﻿// You may obtain a copy of the License at
﻿// http://www.apache.org/licenses/LICENSE-2.0
﻿// Unless required by applicable law or agreed to in writing, software
﻿// distributed under the License is distributed on an "AS IS" BASIS,
﻿// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
﻿// See the License for the specific language governing permissions and
﻿// limitations under the License.
﻿// -----------------------------------------------------------------------------
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Azure.Commands.Batch.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Azure.Batch;
    
    
    public partial class PSCloudPool
    {
        
        internal CloudPool omObject;
        
        private IList<String> applicationLicenses;
        
        private IList<PSApplicationPackageReference> applicationPackageReferences;
        
        private PSAutoScaleRun autoScaleRun;
        
        private IList<PSCertificateReference> certificateReferences;
        
        private PSCloudServiceConfiguration cloudServiceConfiguration;
        
        private IList<PSMetadataItem> metadata;
        
        private PSNetworkConfiguration networkConfiguration;
        
        private IReadOnlyList<PSResizeError> resizeErrors;
        
        private PSStartTask startTask;
        
        private PSPoolStatistics statistics;
        
        private PSTaskSchedulingPolicy taskSchedulingPolicy;
        
        private IList<PSUserAccount> userAccounts;
        
        private PSVirtualMachineConfiguration virtualMachineConfiguration;
        
        internal PSCloudPool(CloudPool omObject)
        {
            if (omObject == null)
            {
                throw new ArgumentNullException("omObject");
            }
            this.omObject = omObject;
        }
        
        public Azure.Batch.Common.AllocationState? AllocationState
        {
            get
            {
                return omObject.AllocationState;
            }
        }
        
        public DateTime? AllocationStateTransitionTime
        {
            get
            {
                return omObject.AllocationStateTransitionTime;
            }
        }
        
        public IList<String> ApplicationLicenses
        {
            get
            {
                if (applicationLicenses == null 
                    && omObject.ApplicationLicenses != null)
                {
                    List<String> list;
                    list = new List<String>();
                    IEnumerator<String> enumerator;
                    enumerator = omObject.ApplicationLicenses.GetEnumerator();
                    for (
                    ; enumerator.MoveNext(); 
                    )
                    {
                        list.Add(enumerator.Current);
                    }
                    applicationLicenses = list;
                }
                return applicationLicenses;
            }
            set
            {
                if (value == null)
                {
                    omObject.ApplicationLicenses = null;
                }
                else
                {
                    omObject.ApplicationLicenses = new List<String>();
                }
                applicationLicenses = value;
            }
        }
        
        public IList<PSApplicationPackageReference> ApplicationPackageReferences
        {
            get
            {
                if (applicationPackageReferences == null 
                    && omObject.ApplicationPackageReferences != null)
                {
                    List<PSApplicationPackageReference> list;
                    list = new List<PSApplicationPackageReference>();
                    IEnumerator<ApplicationPackageReference> enumerator;
                    enumerator = omObject.ApplicationPackageReferences.GetEnumerator();
                    for (
                    ; enumerator.MoveNext(); 
                    )
                    {
                        list.Add(new PSApplicationPackageReference(enumerator.Current));
                    }
                    applicationPackageReferences = list;
                }
                return applicationPackageReferences;
            }
            set
            {
                if (value == null)
                {
                    omObject.ApplicationPackageReferences = null;
                }
                else
                {
                    omObject.ApplicationPackageReferences = new List<ApplicationPackageReference>();
                }
                applicationPackageReferences = value;
            }
        }
        
        public Boolean? AutoScaleEnabled
        {
            get
            {
                return omObject.AutoScaleEnabled;
            }
            set
            {
                omObject.AutoScaleEnabled = value;
            }
        }
        
        public TimeSpan? AutoScaleEvaluationInterval
        {
            get
            {
                return omObject.AutoScaleEvaluationInterval;
            }
            set
            {
                omObject.AutoScaleEvaluationInterval = value;
            }
        }
        
        public string AutoScaleFormula
        {
            get
            {
                return omObject.AutoScaleFormula;
            }
            set
            {
                omObject.AutoScaleFormula = value;
            }
        }
        
        public PSAutoScaleRun AutoScaleRun
        {
            get
            {
                if (autoScaleRun == null 
                    && omObject.AutoScaleRun != null)
                {
                    autoScaleRun = new PSAutoScaleRun(omObject.AutoScaleRun);
                }
                return autoScaleRun;
            }
        }
        
        public IList<PSCertificateReference> CertificateReferences
        {
            get
            {
                if (certificateReferences == null 
                    && omObject.CertificateReferences != null)
                {
                    List<PSCertificateReference> list;
                    list = new List<PSCertificateReference>();
                    IEnumerator<CertificateReference> enumerator;
                    enumerator = omObject.CertificateReferences.GetEnumerator();
                    for (
                    ; enumerator.MoveNext(); 
                    )
                    {
                        list.Add(new PSCertificateReference(enumerator.Current));
                    }
                    certificateReferences = list;
                }
                return certificateReferences;
            }
            set
            {
                if (value == null)
                {
                    omObject.CertificateReferences = null;
                }
                else
                {
                    omObject.CertificateReferences = new List<CertificateReference>();
                }
                certificateReferences = value;
            }
        }
        
        public PSCloudServiceConfiguration CloudServiceConfiguration
        {
            get
            {
                if (cloudServiceConfiguration == null 
                    && omObject.CloudServiceConfiguration != null)
                {
                    cloudServiceConfiguration = new PSCloudServiceConfiguration(omObject.CloudServiceConfiguration);
                }
                return cloudServiceConfiguration;
            }
            set
            {
                if (value == null)
                {
                    omObject.CloudServiceConfiguration = null;
                }
                else
                {
                    omObject.CloudServiceConfiguration = value.omObject;
                }
                cloudServiceConfiguration = value;
            }
        }
        
        public DateTime? CreationTime
        {
            get
            {
                return omObject.CreationTime;
            }
        }
        
        public Int32? CurrentDedicatedComputeNodes
        {
            get
            {
                return omObject.CurrentDedicatedComputeNodes;
            }
        }
        
        public Int32? CurrentLowPriorityComputeNodes
        {
            get
            {
                return omObject.CurrentLowPriorityComputeNodes;
            }
        }
        
        public string DisplayName
        {
            get
            {
                return omObject.DisplayName;
            }
            set
            {
                omObject.DisplayName = value;
            }
        }
        
        public string ETag
        {
            get
            {
                return omObject.ETag;
            }
        }
        
        public string Id
        {
            get
            {
                return omObject.Id;
            }
            set
            {
                omObject.Id = value;
            }
        }
        
        public Boolean? InterComputeNodeCommunicationEnabled
        {
            get
            {
                return omObject.InterComputeNodeCommunicationEnabled;
            }
            set
            {
                omObject.InterComputeNodeCommunicationEnabled = value;
            }
        }
        
        public DateTime? LastModified
        {
            get
            {
                return omObject.LastModified;
            }
        }
        
        public Int32? MaxTasksPerComputeNode
        {
            get
            {
                return omObject.MaxTasksPerComputeNode;
            }
            set
            {
                omObject.MaxTasksPerComputeNode = value;
            }
        }
        
        public IList<PSMetadataItem> Metadata
        {
            get
            {
                if (metadata == null 
                    && omObject.Metadata != null)
                {
                    List<PSMetadataItem> list;
                    list = new List<PSMetadataItem>();
                    IEnumerator<MetadataItem> enumerator;
                    enumerator = omObject.Metadata.GetEnumerator();
                    for (
                    ; enumerator.MoveNext(); 
                    )
                    {
                        list.Add(new PSMetadataItem(enumerator.Current));
                    }
                    metadata = list;
                }
                return metadata;
            }
            set
            {
                if (value == null)
                {
                    omObject.Metadata = null;
                }
                else
                {
                    omObject.Metadata = new List<MetadataItem>();
                }
                metadata = value;
            }
        }
        
        public PSNetworkConfiguration NetworkConfiguration
        {
            get
            {
                if (networkConfiguration == null 
                    && omObject.NetworkConfiguration != null)
                {
                    networkConfiguration = new PSNetworkConfiguration(omObject.NetworkConfiguration);
                }
                return networkConfiguration;
            }
            set
            {
                if (value == null)
                {
                    omObject.NetworkConfiguration = null;
                }
                else
                {
                    omObject.NetworkConfiguration = value.omObject;
                }
                networkConfiguration = value;
            }
        }
        
        public IReadOnlyList<PSResizeError> ResizeErrors
        {
            get
            {
                if (resizeErrors == null 
                    && omObject.ResizeErrors != null)
                {
                    List<PSResizeError> list;
                    list = new List<PSResizeError>();
                    IEnumerator<ResizeError> enumerator;
                    enumerator = omObject.ResizeErrors.GetEnumerator();
                    for (
                    ; enumerator.MoveNext(); 
                    )
                    {
                        list.Add(new PSResizeError(enumerator.Current));
                    }
                    resizeErrors = list.AsReadOnly();
                }
                return resizeErrors;
            }
        }
        
        public TimeSpan? ResizeTimeout
        {
            get
            {
                return omObject.ResizeTimeout;
            }
            set
            {
                omObject.ResizeTimeout = value;
            }
        }
        
        public PSStartTask StartTask
        {
            get
            {
                if (startTask == null 
                    && omObject.StartTask != null)
                {
                    startTask = new PSStartTask(omObject.StartTask);
                }
                return startTask;
            }
            set
            {
                if (value == null)
                {
                    omObject.StartTask = null;
                }
                else
                {
                    omObject.StartTask = value.omObject;
                }
                startTask = value;
            }
        }
        
        public Azure.Batch.Common.PoolState? State
        {
            get
            {
                return omObject.State;
            }
        }
        
        public DateTime? StateTransitionTime
        {
            get
            {
                return omObject.StateTransitionTime;
            }
        }
        
        public PSPoolStatistics Statistics
        {
            get
            {
                if (statistics == null 
                    && omObject.Statistics != null)
                {
                    statistics = new PSPoolStatistics(omObject.Statistics);
                }
                return statistics;
            }
        }
        
        public Int32? TargetDedicatedComputeNodes
        {
            get
            {
                return omObject.TargetDedicatedComputeNodes;
            }
            set
            {
                omObject.TargetDedicatedComputeNodes = value;
            }
        }
        
        public Int32? TargetLowPriorityComputeNodes
        {
            get
            {
                return omObject.TargetLowPriorityComputeNodes;
            }
            set
            {
                omObject.TargetLowPriorityComputeNodes = value;
            }
        }
        
        public PSTaskSchedulingPolicy TaskSchedulingPolicy
        {
            get
            {
                if (taskSchedulingPolicy == null 
                    && omObject.TaskSchedulingPolicy != null)
                {
                    taskSchedulingPolicy = new PSTaskSchedulingPolicy(omObject.TaskSchedulingPolicy);
                }
                return taskSchedulingPolicy;
            }
            set
            {
                if (value == null)
                {
                    omObject.TaskSchedulingPolicy = null;
                }
                else
                {
                    omObject.TaskSchedulingPolicy = value.omObject;
                }
                taskSchedulingPolicy = value;
            }
        }
        
        public string Url
        {
            get
            {
                return omObject.Url;
            }
        }
        
        public IList<PSUserAccount> UserAccounts
        {
            get
            {
                if (userAccounts == null 
                    && omObject.UserAccounts != null)
                {
                    List<PSUserAccount> list;
                    list = new List<PSUserAccount>();
                    IEnumerator<UserAccount> enumerator;
                    enumerator = omObject.UserAccounts.GetEnumerator();
                    for (
                    ; enumerator.MoveNext(); 
                    )
                    {
                        list.Add(new PSUserAccount(enumerator.Current));
                    }
                    userAccounts = list;
                }
                return userAccounts;
            }
            set
            {
                if (value == null)
                {
                    omObject.UserAccounts = null;
                }
                else
                {
                    omObject.UserAccounts = new List<UserAccount>();
                }
                userAccounts = value;
            }
        }
        
        public PSVirtualMachineConfiguration VirtualMachineConfiguration
        {
            get
            {
                if (virtualMachineConfiguration == null 
                    && omObject.VirtualMachineConfiguration != null)
                {
                    virtualMachineConfiguration = new PSVirtualMachineConfiguration(omObject.VirtualMachineConfiguration);
                }
                return virtualMachineConfiguration;
            }
            set
            {
                if (value == null)
                {
                    omObject.VirtualMachineConfiguration = null;
                }
                else
                {
                    omObject.VirtualMachineConfiguration = value.omObject;
                }
                virtualMachineConfiguration = value;
            }
        }
        
        public string VirtualMachineSize
        {
            get
            {
                return omObject.VirtualMachineSize;
            }
            set
            {
                omObject.VirtualMachineSize = value;
            }
        }
    }
}
