using System;

namespace SlimRest.Request.Handlers
{
    public class UrlParameterHandler : IRestRequestHandler
    {
        public UrlParameterHandler(string token, string value)
        {
            Token = token;
            Value = value;
        }

        public string Token { get; set; }
        public string Value { get; set; }

        public void Handle(RestRequest request)
        {
            request.Url = request.Url.Replace(String.Format("{{{0}}}", Token), Value);
        }
    }
}