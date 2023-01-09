using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticClientTest.TestModels
{
    public class Test1
    {
        public Test1(int a)
        {
            Console.WriteLine("single");
        }
        public Test1(int a,int b)
        {
            Console.WriteLine("double");
        }
    }
}
