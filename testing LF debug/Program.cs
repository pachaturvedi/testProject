using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace testing_LF_debug
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var logs = new List<string> { "b" };
            var logs1 = new List<string> { "a1" ,"a2","a3","a4"};
            var logs2 = new List<string> { "a1", "b2", "a3" };
            var logs3 = new List<string> { "a1", "a2", "a3", "a1", "a2", "b3" };
            TF(logs);
            Console.WriteLine();
            TF(logs1);
            Console.WriteLine();
            TF(logs2);
            Console.WriteLine();
            TF(logs3);
            Console.WriteLine();
        }

        public static void TF(List<string> logs)
        {
            var logBatch = CreateBatch();
            var logsEnumerator = logs.GetEnumerator();
            var isLast = !logsEnumerator.MoveNext();
            while (!isLast)
            {
                var log = logsEnumerator.Current;
                isLast = !logsEnumerator.MoveNext();
                if (!logBatch.TryAdd(log))
                {
                    SendAsync(logBatch);
                    logBatch = CreateBatch();
                    if(!logBatch.TryAdd(log))
                    {
                        Console.WriteLine("Log too large");
                    }
                }
                if(isLast)
                {
                    SendAsync(logBatch);
                }
                
            }
        }

        public static void TF2(List<string> logs)
        {
            var logBatch = CreateBatch();
            var logsEnumerator = logs.GetEnumerator();
            var isLast = !logsEnumerator.MoveNext();
            while (!isLast)
            {
                var log = logsEnumerator.Current;
                isLast = !logsEnumerator.MoveNext();
                if (!logBatch.TryAdd(log) || isLast)
                {
                    SendAsync(logBatch);

                    if (!isLast)
                    {
                        logBatch = CreateBatch();
                        if (!logBatch.TryAdd(log))
                        {
                            // a single log whose size is too big to be sent
                            // return RequestEntityTooLarge to LF to avoid being retried
                            Console.WriteLine("Log too large");
                        }
                    }
                }
            }
        }

        public static void SendAsync(LogBatch logbatch)
        {
            foreach (var log in logbatch.logs)
            {
                Console.WriteLine(log);
            }
        }

        public static LogBatch CreateBatch()
        {
            return new LogBatch();
        }

        public class LogBatch
        {
            public List<string> logs;

            public LogBatch()
            {
                logs = new List<string>();
            }

            public bool TryAdd(string log)
            {
                if(log.StartsWith('b'))
                {
                    return false;
                }
                else if(logs.Count() >3 )
                {
                    return false;
                }
                else
                {
                    logs.Add(log);
                    return true;
                }
            }
        }
    }
}
