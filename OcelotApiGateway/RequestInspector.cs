namespace OcelotApiGateway
{
    public class RequestInspector : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"\nDevam etmeden önce şu gelen Request içeriğini bir inceyelim\n{request.ToString()}\n");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
