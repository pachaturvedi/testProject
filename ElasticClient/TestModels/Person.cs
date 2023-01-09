using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ElasticClientTest.TestModels
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public Person()
        {
            Random rnd = new Random();
            Id = rnd.Next();
            FirstName = "f";
            LastName = "l";
        }

        public Person(string name)
        {
            Random rnd = new Random();
            FirstName = name;
            LastName = "l";
        }
    }
}
