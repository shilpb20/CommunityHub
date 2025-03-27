namespace CommunityHub.Api
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public TransactionSettings TransactionSettings { get; set; }
    }

    public class TransactionSettings
    {
        public bool UseInMemoryDatabase { get; set; }
    }
}
