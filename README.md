SlimRest
========

This client is a simple synchronous/asynchronous wrapper for making requests to RESTful APIs. It exposes basic `GET`, `POST`, `PUT`, `PATCH` and `DELETE` requests.

## Client Basics

Given a simple [RESTful API](http://catfacts-api.appspot.com/doc.html), the following retrieves a few cat facts.


    public class CatFactResponse
    {
        public List<string> Facts { get; set; }
        public bool Success { get; set; }
    }

    var client = new RestClient("http://catfacts-api.appspot.com/api");

    var response = client.Get<CatFactResponse>(
                    new RestRequest("facts")
                        .WithQueryParameter("number", 5)
                );
                
## NuGet Package

A NuGet package is available [here](https://nuget.org/packages/SlimRest/).
