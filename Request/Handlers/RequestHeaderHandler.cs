namespace RestEasy.Request.Handlers
{
    public class RequestHeaderHandler
    {
        public RequestHeaderHandler(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}