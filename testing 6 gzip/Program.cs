using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace testing_6_gzip
{
    internal class Program
    {
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





            string filePath = "C:\\Users\\utkarshjain\\Downloads\\sourcedataforerrorlog (1).gz";
            float x1 = 0;
            float x2 = 0;
            float x3 = 0;
            for (int i = 0; i < 1000; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();
                int batch1 = f1(filePath);
                stopwatch.Stop();
                float a = stopwatch.ElapsedTicks;
                Console.WriteLine(batch1);
                Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
                stopwatch.Start();
                int batch2 = f2(filePath).Result;
                stopwatch.Stop();
                float b = stopwatch.ElapsedTicks;
                Console.WriteLine(batch2);
                Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);

                stopwatch.Reset();
                stopwatch.Start();
                int batch3 = f3(filePath).Result;
                stopwatch.Stop();
                float c = stopwatch.ElapsedTicks;
                Console.WriteLine(batch3);
                Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks + " mS: " + stopwatch.ElapsedMilliseconds);

                x1 = x1 + a;
                x2 = x2 + b;
                x3 = x3 + c;
                                
            }
            Console.WriteLine(x1 / 1000);
            Console.WriteLine(x2 / 1000);
            Console.WriteLine(x3 / 1000);
        }

        public static int f1(string filePath)
        {
            using FileStream compressedFileStream = File.OpenRead(filePath);
            using GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            StreamReader file = new StreamReader(decompressionStream);
            string line;
            int uncompressedSize = 0;

            while ((line = file.ReadLine()) != null)
            {
                uncompressedSize += line.Length;
            }
            file.Close();
            return uncompressedSize;
        }

        public static async Task<int> f2(string filePath)
        {
            using FileStream compressedFileStream = File.OpenRead(filePath);
            using FileStream decompressedFileStream = File.Create(filePath + ".dup1");
            using GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            await decompressionStream.CopyToAsync(decompressedFileStream);
            StreamReader file = new StreamReader(filePath + ".dup");
            string line;
            int uncompressedSize = 0;

            while ((line = file.ReadLine()) != null)
            {
                uncompressedSize += line.Length;
            }
            file.Close();
            return uncompressedSize;
        }

        public static async Task<int> f3(string filePath)
        {
            using FileStream compressedFileStream = File.OpenRead(filePath);
            using GZipStream decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            MemoryStream ms = new MemoryStream();
            decompressionStream.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader file = new StreamReader(ms);
            string line;
            int uncompressedSize = 0;

            while ((line = file.ReadLine()) != null)
            {
                uncompressedSize += line.Length;
            }
            file.Close();
            return uncompressedSize;
        }
    }
}
