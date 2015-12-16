using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using SlimRest.Request;

namespace SlimRest.Client
{
    public interface IRestClient
    {
        T Get<T>(RestRequest request);
        Task<T> GetAsync<T>(RestRequest request);

       T Post<T, TK>(RestDataRequest<TK> request);
        Task<T> PostAsync<T, TK>(RestDataRequest<TK> request);

        HttpStatusCode Put<T>(RestDataRequest<T> request);
        Task<HttpStatusCode> PutAsync<T>(RestDataRequest<T> request);
        
        T Put<T, TK>(RestDataRequest<TK> request);
        Task<T> PutAsync<T, TK>(RestDataRequest<TK> request);


        HttpStatusCode Delete(RestRequest request);
        Task<HttpStatusCode> DeleteAsync(RestRequest request);

        HttpStatusCode Delete<T>(RestDataRequest<T> request);
        Task<HttpStatusCode> DeleteAsync<T>(RestDataRequest<T> request);
        
        T Delete<T, TK>(RestDataRequest<TK> request);
        Task<T> DeleteAsync<T, TK>(RestDataRequest<TK> request);

        HttpStatusCode Patch<T>(RestDataRequest<T> request);
        Task<HttpStatusCode> PatchAsync<T>(RestDataRequest<T> request);
        
        T Patch<T, TK>(RestDataRequest<TK> request);
        Task<T> PatchAsync<T, TK>(RestDataRequest<TK> request);
    }

    [DesignerCategory("")]
    public class RestClient: RestWebClient, IRestClient
    {
        public RestClient(string baseUrl, string contentType = "application/json")
            :base(baseUrl, contentType)
        {

        }

        public T Get<T>(RestRequest request)
        {
            var webRequest = BuildRequest(request, WebRequestMethods.Http.Get);
            return HandleResponse<T>(webRequest.GetResponse());
        }

        public async Task<T> GetAsync<T>(RestRequest request)
        {
            var webRequest = BuildRequest(request, WebRequestMethods.Http.Get);
            return await HandleResponseAsync<T>(
                    await webRequest.GetResponseAsync());
        }

        public T Post<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = BuildPostRequest(request);
            return HandleResponse<T>(webRequest.GetResponse());
        }

        public async Task<T> PostAsync<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = await BuildPostRequestAsync(request);
            return await HandleResponseAsync<T>(
                    await webRequest.GetResponseAsync());
        }

        public HttpStatusCode Put<T>(RestDataRequest<T> request)
        {
            var webRequest = BuildPutRequest(request);
            return HandleResponse(webRequest.GetResponse());
        }

        public async Task<HttpStatusCode> PutAsync<T>(RestDataRequest<T> request)
        {
            var webRequest = await BuildPutRequestAsync(request);
            return HandleResponse(await webRequest.GetResponseAsync());

        }

        public T Put<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = BuildPutRequest(request);
            return HandleResponse<T>(webRequest.GetResponse());
        }

        public async Task<T> PutAsync<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = await BuildPutRequestAsync(request);
            return await HandleResponseAsync<T>(
                    await webRequest.GetResponseAsync());
        }

        public HttpStatusCode Delete(RestRequest request)
        {
            var webRequest = BuildRequest(request, "DELETE");
            return HandleResponse(webRequest.GetResponse());
        }

        public async Task<HttpStatusCode> DeleteAsync(RestRequest request)
        {
            var webRequest = BuildRequest(request, "DELETE");
            return HandleResponse(await webRequest.GetResponseAsync());
        }

        public HttpStatusCode Delete<T>(RestDataRequest<T> request)
        {
            var webRequest = BuildDeleteRequest(request);
            return HandleResponse(webRequest.GetResponse());
        }

        public async Task<HttpStatusCode> DeleteAsync<T>(RestDataRequest<T> request)
        {
            var webRequest = await BuildDeleteRequestAsync(request);
            return HandleResponse(await webRequest.GetResponseAsync());
        }

        public T Delete<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = BuildDeleteRequest(request);
            return HandleResponse<T>(webRequest.GetResponse());
        }

        public async Task<T> DeleteAsync<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = await BuildDeleteRequestAsync(request);
            return await HandleResponseAsync<T>(
                    await webRequest.GetResponseAsync());
        }

        public HttpStatusCode Patch<T>(RestDataRequest<T> request)
        {
            var webRequest = BuildPatchRequest(request);
            return HandleResponse(webRequest.GetResponse());
        }

        public async Task<HttpStatusCode> PatchAsync<T>(RestDataRequest<T> request)
        {
            var webRequest = await BuildPatchRequestAsync(request);
            return HandleResponse(await webRequest.GetResponseAsync());

        }

        public T Patch<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = BuildPatchRequest(request);
            return HandleResponse<T>(webRequest.GetResponse());
        }

        public async Task<T> PatchAsync<T, TK>(RestDataRequest<TK> request)
        {
            var webRequest = await BuildPatchRequestAsync(request);
            return await HandleResponseAsync<T>(
                    await webRequest.GetResponseAsync());
        }
    }
}
