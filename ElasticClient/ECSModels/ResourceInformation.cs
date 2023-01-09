//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using Newtonsoft.Json;

namespace ElasticClientTest.ECSModels
{
    public class ResourceInformation
    {
        [JsonProperty(PropertyName = "resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
    }
}
