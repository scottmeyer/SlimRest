using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestEasy.Request.Handlers
{
    public class UrlQueryParameter : IRestRequestHandler
    {
        public UrlQueryParameter(string name, object value)
        {
            Name = name;
            Value = Convert.ToString(value);
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public void Handle(RestRequest request)
        {
            var hasParameters = request.Url.IndexOf('?') != -1;

            if (!hasParameters)
                request.Url += "?";
            
            request.Url = String.Concat(request.Url, String.Format("&{0}={1}", Name, Value));
        }
    }
}
