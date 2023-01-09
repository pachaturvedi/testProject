using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace testing4
{
    internal class Program
    {
        public static HttpClient httpClient;
        public static string adapterEndpoint;
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(2); // Uses the second Core or Processor for the Test
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;      // Prevents "Normal" processes from interrupting Threads
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            stopwatch.Reset();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < 1200)  // A Warmup of 1000-1500 mS 
                                                          // stabilizes the CPU cache and pipeline.
            {
                Task.Delay(2000).Wait();
            }
            stopwatch.Stop();
            string content = "abcd";
            adapterEndpoint = "http://localhost:16064/weatherforecast";
            httpClient = new HttpClient();
            float c = 0;
            for (int i = 0; i < 1000; i++)
            {
                content = content + i.ToString();
                stopwatch.Reset();
                stopwatch.Start();
                var x1=function1(content).Result;
                //Console.WriteLine(function1(content).Result);
                stopwatch.Stop();
                float a = stopwatch.ElapsedTicks;
                Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
                stopwatch.Start();
                var x2=function2(content).Result;
                // Console.WriteLine(function2(content).Result);
                stopwatch.Stop();
                float b = stopwatch.ElapsedTicks;
                Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);
                c=c+ (a/b);

                Console.WriteLine(i);
            }
            Console.WriteLine(c / 10000);
            


        }

        static async Task<string> function1(string payload)
        {
            using (var content = new StringContent(payload, Encoding.UTF8, "application/json"))
            {
                var response = await httpClient.PostAsync(adapterEndpoint, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        static async Task<string> function2(string payload)
        {
            return await Task.FromResult(payload);
        }
    }
}
