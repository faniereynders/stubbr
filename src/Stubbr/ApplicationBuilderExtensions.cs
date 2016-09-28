using Stubbr;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseStubs(this IApplicationBuilder app)
        {
            return app.UseMiddleware<StubMiddleware>();
        }
    }
}