using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stubbr
{
    public class StubMiddleware
    {
        private readonly Dictionary<string, StubResponse> stubOptions;
        private readonly RequestDelegate next;

        public StubMiddleware(RequestDelegate next, IOptions<Dictionary<string, StubResponse>> stubOptions)
        {
            this.next = next;
            this.stubOptions = stubOptions.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var hasStub = stubOptions.ContainsKey($"{context.Request.Method} {context.Request.Path}");
            if (hasStub)
            {
                var stub = stubOptions[$"{context.Request.Method} {context.Request.Path}"];

                byte[] content;
                if (stub.Body.StartsWith("http"))
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(stub.Body);
                        content = await response.Content.ReadAsByteArrayAsync();
                    }
                }
                else
                {
                    content = File.ReadAllBytes($"{stub.Body}");
                }
               
                context.Response.StatusCode = stub.Status;
                
                foreach (var header in stub.Headers)
                {
                    if (context.Response.Headers.ContainsKey(header.Key))
                    {
                        context.Response.Headers[header.Key] = header.Value;
                    }
                    else
                    {
                        context.Response.Headers.Add(header.Key, header.Value);
                    }
                }
                await context.Response.Body.WriteAsync(content,0,content.Length);
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}
