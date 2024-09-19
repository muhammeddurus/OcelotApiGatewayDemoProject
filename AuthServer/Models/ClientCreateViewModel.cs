namespace AuthServer.Models
{
    public class ClientCreateViewModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DisplayName { get; set; }
        public string RedirectUri { get; set; }
    }
}
