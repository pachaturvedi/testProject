//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ElasticClientTest.ECSModels
{
    public class ECSService
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
