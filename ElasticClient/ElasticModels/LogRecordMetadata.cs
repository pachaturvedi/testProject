using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticClientTest.ElasticModels
{
    public class LogRecordMetadata
    {
        [JsonProperty("operationName")]
        public string OperationName { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
