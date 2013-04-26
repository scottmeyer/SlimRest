using System;

namespace SlimRest.Request.Handlers
{
    public class UrlQueryParameter : IRestRequestHandler
    {
        public UrlQueryParameter(string name, object value)
        {
            Name = name;

            if(value!= null)
                Value = Convert.ToString(value);
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public void Handle(RestRequest request)
        {
            var hasParameters = request.Url.IndexOf('?') != -1;

            if (!hasParameters)
            {
                request.Url += "?" + Name;

                if (Value != null)
                {
                    request.Url += "=" + Value;
                }
                return;
            }

            request.Url = String.Concat(request.Url, Value == null 
                ? String.Format("&{0}", Name) 
                : String.Format("&{0}={1}", Name, Value));
        }
    }
}
