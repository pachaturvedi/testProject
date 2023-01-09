//-----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.Serialization;

namespace ElasticClientTest.ECSModels
{
    public class ElasticEvent
    {
        [DataMember(Name = "@timestamp")]
        public string Timestamp { get; set; }

        [DataMember(Name = "agent")]
        public ECSAgent Agent { get; set; }

        [DataMember(Name = "data_stream")]
        public ECSDataStream DataStream { get; set; }

        [DataMember(Name = "input")]
        public ECSInput Input { get; set; }

        [DataMember(Name = "azure_log_forwarder")]
        public ECSLogForwarder AzureLogForwarder { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "tags")]
        public string[] Tags { get; set; }

        [DataMember(Name = "service")]
        public ECSService Service { get; set; }

        [DataMember(Name = "event")]
        public ECSEvent Event { get; set; }
    }
}
