namespace SlimRest.Request.Handlers
{
    public class RestDataHandler<T> : IRestDataRequestHandler<T>
    {
        public RestDataHandler(T data)
        {
            Data = data;
        }
        
        public T Data { get; set; }

        public void Handle(RestDataRequest<T> request)
        {
            request.Data = Data;
        }

        public void Handle(RestRequest request)
        {
            ((RestDataRequest<T>)request).Data = Data;
        }
    }
}
