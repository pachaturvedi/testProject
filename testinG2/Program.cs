using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace testinG2
{
    internal class Program
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }
        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean QueryThreadCycleTime(IntPtr threadHandle, out UInt64 CycleTime);

        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        static void Main(string[] args)
        {
            main();
            Console.ReadKey();
        }
        static async void main()
        {
            await Task.Run(() =>
            {
                long ticksThisTime = 0;
                int inputNum;
                Stopwatch timePerParse;

                switch (0)
                {
                    case 0:
                        // Parse a valid integer using
                        // a try-catch statement.

                        // Start a new stopwatch timer.
                        timePerParse = Stopwatch.StartNew();

                        try
                        {
                            inputNum = Int32.Parse("0");
                        }
                        catch (FormatException)
                        {
                            inputNum = 0;
                        }

                        // Stop the timer, and save the
                        // elapsed ticks for the operation.

                        timePerParse.Stop();
                        ticksThisTime = timePerParse.ElapsedTicks;
                        Console.WriteLine(ticksThisTime);

                        // Parse a valid integer using
                        // the TryParse statement.

                        // Start a new stopwatch timer.
                        timePerParse = Stopwatch.StartNew();

                        if (!Int32.TryParse("0", out inputNum))
                        {
                            inputNum = 0;
                        }
                        Task.Delay(TimeSpan.FromSeconds(3)).Wait();
                        // Stop the timer, and save the
                        // elapsed ticks for the operation.
                        timePerParse.Stop();
                        ticksThisTime = timePerParse.ElapsedTicks;
                        Console.WriteLine(ticksThisTime);

                        // Parse an invalid value using
                        // a try-catch statement.

                        // Start a new stopwatch timer.
                        timePerParse = Stopwatch.StartNew();

                        try
                        {
                            inputNum = Int32.Parse("a");
                        }
                        catch (FormatException)
                        {
                            inputNum = 0;
                        }

                        // Stop the timer, and save the
                        // elapsed ticks for the operation.
                        timePerParse.Stop();
                        ticksThisTime = timePerParse.ElapsedTicks;
                        Console.WriteLine(ticksThisTime);

                        // Parse an invalid value using
                        // the TryParse statement.

                        // Start a new stopwatch timer.
                        timePerParse = Stopwatch.StartNew();

                        if (!Int32.TryParse("a", out inputNum))
                        {
                            inputNum = 0;
                        }

                        // Stop the timer, and save the
                        // elapsed ticks for the operation.
                        timePerParse.Stop();
                        ticksThisTime = timePerParse.ElapsedTicks;
                        Console.WriteLine(ticksThisTime);
                        break;

                    default:
                        break;
                }
                UInt64 CycleTime = 5;
                IntPtr x = OpenThread(ThreadAccess.GET_CONTEXT, false, Thread.CurrentThread.ManagedThreadId);
                QueryThreadCycleTime(x, out CycleTime);
                Console.WriteLine(CycleTime.ToString());
                Console.WriteLine(x);
            });
        }
    }
}
