namespace RestEasy.Request.Handlers
{
    public interface IRestRequestHandler
    {
        void Handle(RestRequest request);
    }
}