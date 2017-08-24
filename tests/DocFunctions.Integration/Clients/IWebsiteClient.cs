namespace DocFunctions.Integration.Clients
{
    public interface IWebsiteClient
    {
        bool UrlExists(string url);
        bool UrlNotFound(string url);
        long UrlSize(string url);
    }
}
