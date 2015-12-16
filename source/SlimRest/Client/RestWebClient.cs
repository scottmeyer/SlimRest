using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using SlimRest.Request;

namespace SlimRest.Client
{
    [DesignerCategory("")]
    public abstract class RestWebClient : WebClient
    {
        protected RestWebClient(string baseUrl, string contentType = "application/json")
        {
            BaseUrl = baseUrl;
            ContentType = contentType;
        }

        private string BaseUrl { get; set; }
        private string ContentType { get; set; }

        private static WebRequest Trace(WebRequest request)
        {
            System.Diagnostics.Trace.WriteLine(
                String.Format("RestClient Request - [{0}] {1}",
                              request.Method,
                              request.RequestUri));
            return request;
        }

        private static WebResponse Trace(WebResponse response)
        {
            System.Diagnostics.Trace.WriteLine(
                String.Format("RestClient Response - [{0}] {1}",
                              ((HttpWebResponse)response).StatusCode,
                              ((HttpWebResponse)response).StatusDescription));
            return response;
        }

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

                foreach (var key in Headers.AllKeys)
                {
                    webRequest.Headers.Add(key, Headers[key]);
                }

                if (webRequest.RequestUri.IsLoopback)
                {
                    webRequest.Proxy = new WebProxy {Address = new Uri("http://localhost:8888")};
                }
            }

            return Trace(webRequest);
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
                var json = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(request.Data));
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

        protected override WebRequest GetWebRequest(Uri address)
        {
            base.GetWebRequest(address);
            var request = WebRequest.Create(address);
            var webRequest = request as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.ContentType = ContentType;
            }

            return webRequest;
        }

        protected virtual HttpStatusCode HandleResponse(WebResponse webResponse)
        {
            return ((HttpWebResponse)Trace(webResponse)).StatusCode;
        }

        protected virtual T HandleResponse<T>(WebResponse webResponse)
        {
            using (var responseStream = Trace(webResponse).GetResponseStream())
            {
                if (responseStream != null)
                {
                    switch (ParseContentType(webResponse.ContentType))
                    {
                        case "text/json":
                        case "application/json":
                        case "application/javascript":
                            return DeserializeJson<T>(responseStream);
                        case "application/xml":
                            return DeserializeXml<T>(responseStream);
                        case "text/html":
                            {
                                try
                                {
                                    return DeserializeJson<T>(responseStream);
                                }
                                catch { }

                                try
                                {
                                    return DeserializeXml<T>(responseStream);
                                }
                                catch { }

                                throw new ArgumentException("Unable to parse content for text/html content.");
                            }
                        default:
                            throw new ArgumentException("Unrecognized Content-Type: " + webResponse.ContentType);
                    }
                }
            }

            return default(T);
        }

        protected virtual async Task<T> HandleResponseAsync<T>(WebResponse webResponse)
        {
            using (var responseStream = Trace(webResponse).GetResponseStream())
            {
                if (responseStream != null)
                {
                    switch (ParseContentType(webResponse.ContentType))
                    {
                        case "text/json":
                        case "application/json":
                        case "application/javascript":
                            return await DeserializeJsonAsync<T>(responseStream);
                        case "application/xml":
                            return await DeserializeXmlAsync<T>(responseStream);
                        case "text/html":
                            {
                                try
                                {
                                    return await DeserializeJsonAsync<T>(responseStream);
                                }catch{}

                                try
                                {
                                    return await DeserializeXmlAsync<T>(responseStream);
                                }
                                catch{}

                                throw new ArgumentException("Unable to parse content for text/html content.");
                            }
                        default:
                            throw new ArgumentException("Unrecognized Content-Type: " + webResponse.ContentType);
                    }
                }
            }

            return default(T);

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
                var content = Encoding.UTF8.GetString(contentStream.ToArray());
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        internal static async Task<T> DeserializeJsonAsync<T>(Stream jsonStream)
        {
            using (var contentStream = new MemoryStream())
            {
                await jsonStream.CopyToAsync(contentStream);
                var content = Encoding.UTF8.GetString(contentStream.ToArray());
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(content));
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