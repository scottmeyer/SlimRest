using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using RestEasy.Request;

namespace RestEasy.Client
{
    public abstract class RestWebClient
    {
        protected RestWebClient(string baseUrl)
        {
            BaseUrl = baseUrl;
            ContentType = "application/json";
        }

        private string BaseUrl { get; set; }
        private string ContentType { get; set; }

        public CookieContainer Cookies { get; set; }

        protected WebRequest BuildRequest(RestRequest request, string method)
        {
            request.Apply();
            var webRequest = GetWebRequest(new Uri(String.Format("{0}/{1}", BaseUrl, request.Url))) as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.Method = method;
                foreach (var kvp in request.Headers)
                {
                    webRequest.Headers[kvp.Key] = kvp.Value;
                }

                
            }

            return webRequest;
        }

        protected WebRequest BuildDataRequest<T>(RestDataRequest<T> request, string method)
        {
            var webRequest = BuildRequest(request, method);

            using (var requestStream = new StreamWriter(webRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(request.Data);
                requestStream.Write(json);
            }

            return webRequest;
        }

        protected async Task<WebRequest> BuildDataRequestAsync<T>(RestDataRequest<T> request, string method)
        {
            var webRequest = BuildRequest(request, method);
            using (var requestStream = new StreamWriter(webRequest.GetRequestStream()))
            {
                var json = await JsonConvert.SerializeObjectAsync(request.Data);
                await requestStream.WriteAsync(json);
            }

            return webRequest;
        }

        protected WebRequest BuildPostRequest<T>(RestDataRequest<T> request)
        {
            return BuildDataRequest(request, WebRequestMethods.Http.Post);
        }

        protected WebRequest BuildPutRequest<T>(RestDataRequest<T> request)
        {
            return BuildDataRequest(request, WebRequestMethods.Http.Put);
        }

        protected WebRequest BuildDeleteRequest<T>(RestDataRequest<T> request)
        {
            return BuildDataRequest(request, "DELETE");
        }

        protected WebRequest BuildPatchRequest<T>(RestDataRequest<T> request)
        {
            return BuildDataRequest(request, "PATCH");
        }

        protected async Task<WebRequest> BuildPostRequestAsync<T>(RestDataRequest<T> request)
        {
            return await BuildDataRequestAsync(request, WebRequestMethods.Http.Post);
        }

        protected async Task<WebRequest> BuildPutRequestAsync<T>(RestDataRequest<T> request)
        {
            return await BuildDataRequestAsync(request, WebRequestMethods.Http.Put);
        }

        protected async Task<WebRequest> BuildDeleteRequestAsync<T>(RestDataRequest<T> request)
        {
            return await BuildDataRequestAsync(request, "DELETE");
        }

        protected async Task<WebRequest> BuildPatchRequestAsync<T>(RestDataRequest<T> request)
        {
            return await BuildDataRequestAsync(request, "PATCH");
        }

        protected WebRequest GetWebRequest(Uri address)
        {
            var request = WebRequest.Create(address);
            var webRequest = request as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.ContentType = ContentType;
                webRequest.CookieContainer = Cookies;
            }
            return webRequest;
        }

        protected virtual T HandleResponse<T>(WebResponse webResponse)
        {
            var result = default(T);

            using (var responseStream = webResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    switch (ParseContentType(webResponse.ContentType))
                    {
                        case "text/json":
                        case "application/json":
                        case "application/javascript":
                            result = DeserializeJson<T>(responseStream);
                            break;
                        case "application/xml":
                            result = DeserializeXml<T>(responseStream);
                            break;
                        default:
                            throw new ArgumentException("Unrecognized Content-Type: " + webResponse.ContentType);
                    }
                }
            }

            return result;
        }

        protected virtual async Task<T> HandleResponseAsync<T>(WebResponse webResponse)
        {
            var result = default(T);
            using (var responseStream = webResponse.GetResponseStream())
            {
                if (responseStream != null)
                {
                    switch (ParseContentType(webResponse.ContentType))
                    {
                        case "text/json":
                        case "application/json":
                        case "application/javascript":
                            result = await DeserializeJsonAsync<T>(responseStream);
                            break;
                        case "application/xml":
                            result = await DeserializeXmlAsync<T>(responseStream);

                            break;
                        default:
                            throw new ArgumentException("Unrecognized Content-Type: " + webResponse.ContentType);
                    }
                }
            }

            return result;
        }

        protected virtual string ParseContentType(string contentType)
        {
            return contentType.Split(';')[0];
        }

        internal static T DeserializeJson<T>(Stream jsonStream)
        {
            using (var contentStream = new MemoryStream())
            {
                jsonStream.CopyTo(contentStream);
                var content = System.Text.Encoding.UTF8.GetString(contentStream.ToArray());
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        internal static async Task<T> DeserializeJsonAsync<T>(Stream jsonStream)
        {
            using (var contentStream = new MemoryStream())
            {
                await jsonStream.CopyToAsync(contentStream);
                var content = System.Text.Encoding.UTF8.GetString(contentStream.ToArray());
                return await JsonConvert.DeserializeObjectAsync<T>(content);
            }
        }

        internal static T DeserializeXml<T>(Stream xmlStream)
        {
            using (var xmlReader = XmlReader.Create(xmlStream))
            {
                var serializer = new DataContractSerializer(typeof(T));
                var theObject = (T)serializer.ReadObject(xmlReader);
                return theObject;
            }

        }

        internal static async Task<T> DeserializeXmlAsync<T>(Stream xmlStream)
        {
            return await Task.Run(() =>
            {
                using (var xmlReader = XmlReader.Create(xmlStream))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    var theObject = (T)serializer.ReadObject(xmlReader);
                    return theObject;
                }
            });
        }

    }
}