//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace ElasticClientTest.ECSModels
{
    public class ECSEvent
    {
        [DataMember(Name = "module")]
        public string Module { get; set; }

        [DataMember(Name = "dataset")]
        public string Dataset { get; set; }
    }
}
