namespace SlimRest.Request.Handlers
{
    public interface IRestRequestHandler
    {
        void Handle(RestRequest request);
    }

    public interface IRestDataRequestHandler<T> : IRestRequestHandler
    {
        void Handle(RestDataRequest<T> request);
    }
}