using RestEasy.Request.Handlers;

namespace RestEasy.Request
{
    public static class FluentRestRequest
    {
        public static RestRequest WithParameter(this RestRequest request, string token, string value)
        {
            request.RequestHandlers.Add(new UrlParameterHandler(token, value));
            return request;
        }

        public static RestRequest WithHeader(this RestRequest request, string name, string value)
        {
            request.Headers[name] = value;
            return request;
        }

        public static RestRequest WithQueryParameter(this RestRequest request, string name, object value)
        {
            request.RequestHandlers.Add(new UrlQueryParameter(name, value));
            return request;
        }
    }
}