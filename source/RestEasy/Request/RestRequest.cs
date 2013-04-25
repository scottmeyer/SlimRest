using System.Collections.Generic;
using SlimRest.Request.Handlers;

namespace SlimRest.Request
{
    public class RestRequest
    {
        public RestRequest(string url)
        {
            RequestHandlers = new List<IRestRequestHandler>();
            Headers = new Dictionary<string, string>();
            Url = url;
        }

        internal List<IRestRequestHandler> RequestHandlers { get; set; }
        internal Dictionary<string, string> Headers { get; set; } 
        internal virtual RestRequest Apply()
        {
            RequestHandlers.ForEach(x => x.Handle(this));
            return this;
        }
        
        public string Url { get; set; }
    }

    public class RestDataRequest<T> : RestRequest
    {
        public RestDataRequest(string url)
            : base(url)
        {
            
        }

        public RestDataRequest(string url, T data)
            :base(url)
        {
            Data = data;
        }
            
        public T Data { get; set; }
       
    }
}