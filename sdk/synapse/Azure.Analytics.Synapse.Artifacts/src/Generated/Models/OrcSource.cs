// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace Azure.Analytics.Synapse.Artifacts.Models
{
    /// <summary> A copy activity ORC source. </summary>
    public partial class OrcSource : CopySource
    {
        /// <summary> Initializes a new instance of OrcSource. </summary>
        public OrcSource()
        {
            Type = "OrcSource";
        }

        /// <summary> Initializes a new instance of OrcSource. </summary>
        /// <param name="type"> Copy source type. </param>
        /// <param name="sourceRetryCount"> Source retry count. Type: integer (or Expression with resultType integer). </param>
        /// <param name="sourceRetryWait"> Source retry wait. Type: string (or Expression with resultType string), pattern: ((\d+)\.)?(\d\d):(60|([0-5][0-9])):(60|([0-5][0-9])). </param>
        /// <param name="maxConcurrentConnections"> The maximum concurrent connection count for the source data store. Type: integer (or Expression with resultType integer). </param>
        /// <param name="additionalProperties"> . </param>
        /// <param name="storeSettings"> ORC store settings. </param>
        internal OrcSource(string type, object sourceRetryCount, object sourceRetryWait, object maxConcurrentConnections, IDictionary<string, object> additionalProperties, StoreReadSettings storeSettings) : base(type, sourceRetryCount, sourceRetryWait, maxConcurrentConnections, additionalProperties)
        {
            StoreSettings = storeSettings;
            Type = type ?? "OrcSource";
        }

        /// <summary> ORC store settings. </summary>
        public StoreReadSettings StoreSettings { get; set; }
    }
}
