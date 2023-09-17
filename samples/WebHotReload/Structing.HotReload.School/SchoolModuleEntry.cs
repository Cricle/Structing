using Structing.AspNetCore.Annotations;
using Structing.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace Structing.HotReload.School
{
    [EnableApplicationPart]
    public class SchoolModuleEntry : AutoModuleEntry
    {
        public override Task ReadyAsync(IReadyContext context)
        {
            context.GetIEndpointRouteBuilder()
                .MapGet("/aa", async b =>
                {
                    await b.Response.WriteAsync("hello");
                });
            return base.ReadyAsync(context);
        }
    }
}
