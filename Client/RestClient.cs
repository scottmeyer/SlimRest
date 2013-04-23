using System.Net;
using System.Threading.Tasks;
using RestEasy.Request;

namespace RestEasy.Client
{
    public interface IRestClient
    {
        T Get<T>(RestRequest request);
        Task<T> GetAsync<T>(RestRequest request);

        T Post<T>(RestRequest request);
        Task<T> PostAsync<T>(RestRequest request);
        T Post<T, TK>(RestDataRequest<TK> request);
        Task<T> PostAsync<T, TK>(RestDataRequest<TK> request);

        T Put<T, TK>(RestDataRequest<TK> request);
        Task<T> PutAsync<T, TK>(RestDataRequest<TK> request);

        T Delete<T, TK>(RestDataRequest<TK> request);
        Task<T> DeleteAsync<T, TK>(RestDataRequest<TK> request);

        T Patch<T, TK>(RestDataRequest<TK> request);
        Task<T> PatchAsync<T, TK>(RestDataRequest<TK> request);
    }

    public class RestClient: RestWebClient, IRestClient
    {
        public RestClient(string baseUrl)
            :base(baseUrl)
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

        public T Post<T>(RestRequest request)
        {
            return HandleResponse<T>(
                BuildRequest(request, WebRequestMethods.Http.Post)
                .GetResponse()
            );
        }

        public async Task<T> PostAsync<T>(RestRequest request)
        {
            return await HandleResponseAsync<T>(
                    await BuildRequest(request, WebRequestMethods.Http.Post)
                    .GetResponseAsync()
            );
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
