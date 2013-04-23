using System;
using System.Collections.Generic;
using RestEasy.Client;
using RestEasy.Request;

namespace RestEasy.Example
{
    static class Program
    {
        public class CatFactResponse
        {
            public List<string> Facts { get; set; }
            public bool Success { get; set; }
        }

        static void Main()
        {

            var client = new RestClient("http://catfacts-api.appspot.com/api");

            var response = client.Get<CatFactResponse>(
                    new RestRequest("facts")
                        .WithQueryParameter("number", 5)
                
                );

            Console.WriteLine("Retrieved " + response.Facts.Count + " cat facts.");
            Console.WriteLine(response.Facts[0]);

            Console.ReadKey();
        }
    }
}
