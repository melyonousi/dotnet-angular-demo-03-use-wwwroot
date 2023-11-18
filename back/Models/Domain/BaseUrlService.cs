namespace back.Models.Domain
{
    public class BaseUrlService
    {
        public string BaseUrl { get; }
        public BaseUrlService(string baseUrl)
        {
            BaseUrl = baseUrl;
        }
    }
}
