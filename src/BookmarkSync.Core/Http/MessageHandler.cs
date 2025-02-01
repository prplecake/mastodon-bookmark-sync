using System.Net;

namespace BookmarkSync.Core.Http;

public class MessageHandler : DelegatingHandler
{
    public MessageHandler() : base(new HttpClientHandler()) { }
    public MessageHandler(WebProxy? proxy) : base(new HttpClientHandler {Proxy = proxy}) { }
    private static readonly ILogger Logger = Log.ForContext<MessageHandler>();
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Logger.Verbose("Request: {@Request}", request);
        var response = await base.SendAsync(request, cancellationToken);
        Logger.Verbose("Response: {@Response}", response);
        return response;
    }
}
