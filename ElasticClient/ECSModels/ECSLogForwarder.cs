//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ElasticClientTest.ECSModels
{
    public class ECSLogForwarder
    {
        [DataMember(Name = "category")]
        public string Category { get; set; }

        [DataMember(Name = "service_provider")]
        public string ServiceProvider { get; set; }

        [DataMember(Name = "resource_type")]
        public string ResourceType { get; set; }
    }
}
