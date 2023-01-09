using ElasticClientTest.ECSModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticClientTest.ElasticModels
{
    public class ElasticRecordFormatter
    {
        private static ECSAgent s_agent = new ECSAgent
        {
            Name = "MicrosoftAgent",
            Version = "1.0.0",
        };

        private static string[] s_tags = new string[]
        {
            "forwarded",
        };

        private static ECSService s_service = new ECSService
        {
            Type = "azure",
        };

        private static ECSInput s_input = new ECSInput
        {
            Type = "azure-log-forwarder",
        };

        private ECSEvent _event;

        private ECSDataStream _dataStream;

        public ElasticRecordFormatter(string index)
        {
            _event = new ECSEvent
            {
                Module = "azure",
                Dataset = "azure.platformlogs",
            };
            _dataStream = new ECSDataStream
            {
                Namespace = "default",
                Type = "logs",
                Dataset = "azure.platformlogs",
            };
            if (index.Equals(IndexDeciderUtils.s_activityIndex, StringComparison.OrdinalIgnoreCase))
            {
                _event.Dataset = "azure.activitylogs";
                _dataStream.Dataset = "azure.activitylogs";
            }
            else if (index.Equals(IndexDeciderUtils.s_auditIndex, StringComparison.OrdinalIgnoreCase))
            {
                _event.Dataset = "azure.auditlogs";
                _dataStream.Dataset = "azure.auditlogs";
            }
            else if (index.Equals(IndexDeciderUtils.s_signinIndex, StringComparison.OrdinalIgnoreCase))
            {
                _event.Dataset = "azure.signinlogs";
                _dataStream.Dataset = "azure.signinlogs";
            }
        }

        public ElasticEvent Format(string logEvent)
        {
            try
            {
                ECSLogForwarder eCSLogForwarder = ParseAzureLogForwarder(logEvent);

                var ecsEvent = new ElasticEvent
                {
                    Timestamp = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Agent = s_agent,
                    Message = logEvent,
                    AzureLogForwarder = eCSLogForwarder,
                    Tags = s_tags,
                    Input = s_input,
                    DataStream = _dataStream,
                    Service = s_service,
                    Event = _event,
                };
                return ecsEvent;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Log.Error("Failed to format elastic record:{@record} exception:{@x}", logEvent, e);
                throw;
            }
        }

        // sample logs can be found here https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/resource-logs
        // top level schema https://docs.microsoft.com/en-us/azure/azure-monitor/essentials/resource-logs-schema
        private ECSLogForwarder ParseAzureLogForwarder(string log)
        {
            LogRecordMetadata metaData = null;
            try
            {
                metaData = JsonConvert.DeserializeObject<LogRecordMetadata>(log);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Log.Error("Failed to deserialize elastic record:{@record} exception:{@x}", log, e);

                // incase if the record is not a valid json then we should ingest to the default index with default data the exception of parsing is logged in controller also so not logging here
                return new ECSLogForwarder { Category = string.Empty, ServiceProvider = string.Empty, ResourceType = string.Empty };
            }

            string serviceProvider = string.Empty;
            string resourceType = string.Empty;
            try
            {
                if (metaData.OperationName != null)
                {
                    var endIndex = metaData.OperationName.LastIndexOf('/');
                    if (endIndex != -1)
                    {
                        var split = metaData.OperationName.Split("/", StringSplitOptions.RemoveEmptyEntries);
                        if (split.Length >= 1)
                        {
                            serviceProvider = split[0];
                        }

                        resourceType = metaData.OperationName.Substring(0, endIndex);
                    }
                    else
                    {
                        // Log.Warning("Operation Name:{operationName}is not in Expected format ", metaData.OperationName);
                        serviceProvider = metaData.OperationName;
                        resourceType = metaData.OperationName;
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Log.Error("Operation Name is not in Expected operationName:{operationName} exception:{@x}", metaData.OperationName, e);

                // incase if the record is not a valid json then we should ingest to the default index with default data the exception of parsing is logged in controller also so not logging here
                return new ECSLogForwarder { Category = string.Empty, ServiceProvider = string.Empty, ResourceType = string.Empty };
            }

            var data = new ECSLogForwarder
            {
                Category = metaData.Category,
                ServiceProvider = serviceProvider,
                ResourceType = resourceType,
            };
            return data;
        }
    }
}
