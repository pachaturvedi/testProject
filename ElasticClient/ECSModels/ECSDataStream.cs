//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ElasticClientTest.ECSModels
{
    public class ECSDataStream
    {
        [DataMember(Name = "namespace")]
        public string Namespace { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "dataset")]
        public string Dataset { get; set; }
    }
}