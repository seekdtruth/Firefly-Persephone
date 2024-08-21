using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace UnitTests
{
    public class HttpTestRequest : HttpRequest
    {
        private IQueryCollection _queryCollection = new QueryCollection();

        public HttpTestRequest()
        {
            Body = new MemoryStream();
            Method = "";
            Scheme = "http";
            Protocol = "https";
            ContentType = "text/plain";
        }

        public override HttpContext HttpContext => throw new NotImplementedException();

        public override string Method { get; set; }
        public override string Scheme { get; set; }
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; }
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override QueryString QueryString { get; set; }
        public override IQueryCollection Query { get => _queryCollection; set => value = _queryCollection; }
        public override string Protocol { get; set; }

        public override IHeaderDictionary Headers { get; } = new HeaderDictionary();

        public override IRequestCookieCollection Cookies { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override Stream Body { get; set; }

        public override bool HasFormContentType { get; } = false;

        public override IFormCollection Form { get; set; }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IFormCollection>(new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>()));
        }
    }
}
