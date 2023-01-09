using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using ElasticClientTest.ECSModels;
using ElasticClientTest.ElasticModels;
using ElasticClientTest.TestModels;
using Elasticsearch.Net;
using Nest;
using Newtonsoft.Json;

namespace ElasticClientTest
{
    internal class Program
    {
        static Random rnd = new Random();

        public static ElasticClient ESCCreator(string id, string apikey, string indexName = "", int type = 1)
        {
            ConnectionSettings setting = null;

            if (type == 1)
            {
                setting = new ConnectionSettings(new Uri(id)).ApiKeyAuthentication(new ApiKeyAuthenticationCredentials(apikey));
            }
            else
            {
                setting = new ConnectionSettings(id, new ApiKeyAuthenticationCredentials(apikey));
            }
            setting.DefaultMappingFor<Person>(m => m.IndexName("person"));
            setting.EnableApiVersioningHeader();

            // setting.DisableDirectStreaming();  //used for debugging

            if (indexName != "")
            {
                setting.DefaultMappingFor<ElasticEvent>(m => m.IndexName(indexName));
            }

            var client = new ElasticClient(setting);
            return client;
        }

        public static (LogRecordMetadata, string) getLog()
        {
            // string filePath = "C:\\Users\\utkarshjain\\Downloads\\sample log files (1)\\sample log files\\web\\da12abe5d74242d3a76ef5c7e19b78f5~";
            string filePath = "C:\\Users\\utkarshjain\\Downloads\\sample log files (1)\\sample log files\\cosmos\\15a6239ff5e14ade824b437b13b765dd~";

            string[] lines = File.ReadAllLines(filePath);
            List<string> logs = lines.ToList();
            int r = rnd.Next(1, logs.Count);
            LogRecordMetadata logRecordMetadata = JsonConvert.DeserializeObject<LogRecordMetadata>(logs[0]);
            return (logRecordMetadata, logs[r]);
        }

        public static (LogRecordMetadata, List<string>) getLogs()
        {
            string filePath = "C:\\Users\\utkarshjain\\Downloads\\sample log files (1)\\sample log files\\web\\da12abe5d74242d3a76ef5c7e19b78f5~";
            // string filePath = "C:\\Users\\utkarshjain\\Downloads\\sample log files (1)\\sample log files\\cosmos\\15a6239ff5e14ade824b437b13b765dd~";

            string[] lines = File.ReadAllLines(filePath);
            List<string> logs = lines.ToList();

            LogRecordMetadata logRecordMetadata = JsonConvert.DeserializeObject<LogRecordMetadata>(logs[0]);

            logs.RemoveAt(0);
            return (logRecordMetadata, logs);
        }

        static void Main(string[] args)
        {
            (var logRecordMetadata, var logs) = getLogs();
            var indexName = IndexDeciderUtils.GetIndexName(logRecordMetadata);

            /*
            var ingestUri = "https://5dfd635c826f4e318a6341fd915d6fc0.eastus2.staging.azure.foundit.no/";
            var apiKey = "Z1FPTFFvRUJ2bDM0d205Nk1nMzA6alV5YWZfZVBUYWFaZldQNG9Bb2FyUQ==";
            var client = ESCCreator(ingestUri, apiKey);
            */

            
            var ingestUri = "https://0aea3975726b40a0b1908d87609e5893.eastus2.staging.azure.foundit.no/";
            var apiKey = "SzJFYjZvRUJVWXJKTjdNSjRBN3I6cHBhdDJldE5UVGFuYWJJUzlqX2JfQQ==";
            var client = ESCCreator(ingestUri, apiKey);
            

            /*
            var cloudId = "3105westus2:d2VzdHVzMi5henVyZS5lbGFzdGljLWNsb3VkLmNvbTo0NDMkOWQ2MDcxZjIwNjViNGMxYzg2YmZmYzUwYzA4OTZmZTQkMzAwMDMxNzM4YTM1NDMwYjk4YjY5YTcwZGQzZWU0ZWU=";
            var apiKey = "cGpPdllvRUIwWXpSREVxcFBrQzQ6czI5bms4cFVTT0c0QTFHQ2o0dXJ6UQ==x";
            var client = ESCCreator(cloudId, apiKey, type: 2);
            */


            ElasticRecordFormatter elasticRecordFormatter = new ElasticRecordFormatter(indexName);
            List<ElasticEvent> elasticEvents = logs.ConvertAll(x => elasticRecordFormatter.Format(x));

            var batchSuccessful = true;
            var bulkAllObservable = client.BulkAll(elasticEvents, b => b
                .Index(indexName)
                .BufferToBulk((descriptor,buffer) =>
                {
                    foreach (var elasticEvent in buffer)
                    {
                        descriptor.Create<ElasticEvent>(bi => bi
                            .Document(elasticEvent)
                        );
                    }
                })
                .RefreshOnCompleted()
                .MaxDegreeOfParallelism(Environment.ProcessorCount)
                .Size(10)
                .DroppedDocumentCallback((bulkResponseItem, elasticEvent) =>
                {
                    batchSuccessful = false;
                    var error = bulkResponseItem.Error;
                    Console.WriteLine($"Unable to index: {bulkResponseItem} {elasticEvent}");
                })
                .ContinueAfterDroppedDocuments(false)
            );

            var waitHandle = new ManualResetEvent(false);
            ExceptionDispatchInfo exceptionDispatchInfo = null;
            var observer = new BulkAllObserver(
                onNext: response =>
                {
                    // do something e.g. write number of pages to console
                    Console.WriteLine(response.Items.Count);
                },
                onError: exception =>
                {
                    exceptionDispatchInfo = ExceptionDispatchInfo.Capture(exception);
                    Console.WriteLine("hello");
                    waitHandle.Set();
                },
                onCompleted: () => waitHandle.Set()
            );
            bulkAllObservable.Subscribe(observer);
            waitHandle.WaitOne();
            try
            {
                exceptionDispatchInfo?.Throw();
            }
            catch (ElasticsearchClientException e)
            {
                var exception = JsonConvert.SerializeObject(e);
                throw;
            }

            /*
            ElasticEvent doc = elasticRecordFormatter.Format(log);

            var indexResponse =client.Index(doc, i => i.Index(indexName));
            var indexDebugResponse = indexResponse.ApiCall.DebugInformation;
            var stausResponse = indexResponse.ApiCall.HttpStatusCode;
            Console.WriteLine(indexResponse);

            var searchResponse = client.Search<ElasticEvent>(s => s
                .Query(q => q.MatchAll()).Size(100)
            );
            var documents = searchResponse.Documents;
            Console.WriteLine(documents.Count);
            */
        }
    }
}
