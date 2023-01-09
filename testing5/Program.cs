using System;
using System.Net.Http;
using System.Text;
using Microsoft.Liftr;
using Microsoft.Liftr.Contracts;

namespace testing5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ResourceId res = new ResourceId("/SUBSCRIPTIONS/2569177C-DEA3-4F3F-976C-2B23D2A07D40".ToLower());
            //ResourceId res = new ResourceId("/SUBSCRIPTIONS/2569177C-DEA3-4F3F-976C-2B23D2A07D40/RESOURCEGROUPS/DEFAULT-APPLICATIONINSIGHTS-EASTUS/PROVIDERS/MICROSOFT.INSIGHTS/ACTIVITYLOGALERTS/HONEYWELL".ToLower());

            //ResourceId res = new ResourceId("164e4f2d-b3fc-4936-92f3-733af6006ed2".ToLower());
            //ResourceId res = new ResourceId("/tenants/164e4f2d-b3fc-4936-92f3-733af6006ed2/providers/Microsoft.aadiam".ToLower());

            //ResourceId res = new ResourceId("/SUBSCRIPTIONS/CFA0D19A-0073-43BF-90AF-BCD5E73DD93D/RESOURCEGROUPS/SALESSOLUTIONS-RG/PROVIDERS/MICROSOFT.WEB/SITES/T2BLUEDIRECT".ToLower());
            //ResourceId res = new ResourceId("t2bluedirect.publicnonprod-usc-ase1.appserviceenvironment.net".ToLower());

            // AKS does not have resourceid fielad in logs
            //Console.WriteLine(res); 

            var x = new StringContent("Hello world", Encoding.UTF8, "application/json");
            //int value = x;
            Console.WriteLine(x);
        }
    }
}
