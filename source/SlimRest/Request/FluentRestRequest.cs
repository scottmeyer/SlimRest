using System;
using System.Linq.Expressions;
using System.Reflection;
using SlimRest.Request.Handlers;

namespace SlimRest.Request
{
    public static class FluentRestRequest
    {
        public static RestRequest WithParameter(this RestRequest request, string token, string value)
        {
            request.RequestHandlers.Add(new UrlParameterHandler(token, value));
            return request;
        }

        public static RestRequest WithParameter<T>(this RestRequest request, Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            request.RequestHandlers.Add(new UrlParameterHandler(body.Member.Name, Convert.ToString(((FieldInfo)body.Member).GetValue(((ConstantExpression)body.Expression).Value))));
            return request;
        }

        public static RestRequest WithHeader(this RestRequest request, string name, string value)
        {
            request.Headers[name] = value;
            return request;
        }

        public static RestRequest WithQueryParameter(this RestRequest request, string name, object value = null)
        {
            request.RequestHandlers.Add(new UrlQueryParameter(name, value));
            return request;
        }

        public static RestDataRequest<T> WithParameter<T>(this RestDataRequest<T> request, string token, string value)
        {
            WithParameter(request as RestRequest, token, value);
            return request;
        }

        public static RestDataRequest<TK> WithParameter<T, TK>(this RestDataRequest<TK> request, Expression<Func<T>> expr)
        {
            WithParameter(request as RestRequest, expr);
            return request;
        }

        public static RestDataRequest<T> WithHeader<T>(this RestDataRequest<T> request, string name, string value)
        {
            WithHeader(request as RestRequest, name, value);
            return request;
        }

        public static RestDataRequest<T> WithQueryParameter<T>(this RestDataRequest<T> request, string name, object value = null)
        {
            WithQueryParameter(request as RestRequest, name, value);
            return request;
        }

        public static RestDataRequest<T> WithData<T>(this RestDataRequest<T> request, T data)
        {
            request.RequestHandlers.Add(new RestDataHandler<T>(data));
            return request;
        }
    }
}