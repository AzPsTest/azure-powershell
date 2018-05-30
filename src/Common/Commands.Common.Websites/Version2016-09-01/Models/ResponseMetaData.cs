// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.WebSites.Version2016_09_01.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ResponseMetaData
    {
        /// <summary>
        /// Initializes a new instance of the ResponseMetaData class.
        /// </summary>
        public ResponseMetaData()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ResponseMetaData class.
        /// </summary>
        /// <param name="dataSource">Source of the Data</param>
        public ResponseMetaData(DataSource dataSource = default(DataSource))
        {
            DataSource = dataSource;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets source of the Data
        /// </summary>
        [JsonProperty(PropertyName = "dataSource")]
        public DataSource DataSource { get; set; }

    }
}
