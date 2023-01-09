using Microsoft.Liftr;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TEsting;

namespace testing3
{
    internal class Program
    {
        public static StringBuilder sb = new StringBuilder();
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





            string filePath = "C:\\Users\\utkarshjain\\Downloads\\sourcedataforerrorlog (1)";
            StreamReader file = new StreamReader(filePath);
            List<string> lines = new List<string>();
            file.ReadLine();
            lines.Add(file.ReadLine());
            lines.Add(file.ReadLine());
            float c = 0;
            for (int i = 0; i < 1000; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                List<string> batch1 = f1(new List<string>(lines));
                stopwatch.Stop();
                float a = stopwatch.ElapsedTicks;
                Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
                stopwatch.Start();
                List<string> batch2 = f2(new List<string>(lines));
                stopwatch.Stop();
                float b = stopwatch.ElapsedTicks;
                Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);
                c=c+ (a/b);

                Console.WriteLine(lines[0] == batch1[0]);
                Console.WriteLine(lines[1] == batch1[1]);
                Console.WriteLine(lines[0] == batch2[0]);
                Console.WriteLine(lines[1] == batch2[1]);
            }
            Console.WriteLine(c / 1000);
            


            
        }
        static List<string> f1(List<string> lines)
        {
            string temp=lines.ToJsonString();
            return JsonConvert.DeserializeObject<List<string>>(temp);
        }

        static List<string> f2(List<string> lines)
        {
            sb.Clear();
            foreach (var line in lines)
            {
                sb.Append(line);
                sb.Append("\n");
            }
            string temp=sb.ToString();
            var result = temp.Split('\n');
            List<string> ans = new List<string>();
            foreach (var res in result)
            {
                if(res.Length>0)
                    ans.Add(res);
            }
            return ans;
        }

        internal static bool TryExtractLiftrMetadata(string line, out ShoeboxEventMetaData metaData)
        {
            metaData = null;
            if (!line.OrdinalContains("LiftrMetadata"))
            {
                return false;
            }

            try
            {
                var parsed = line.FromJson<ShoeboxEventMetaData>();
                if (!parsed.LiftrMetadata)
                {
                    return false;
                }

                metaData = parsed;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
