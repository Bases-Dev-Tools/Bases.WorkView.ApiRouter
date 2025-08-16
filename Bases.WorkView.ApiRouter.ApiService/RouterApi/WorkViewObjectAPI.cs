

using Bases.WorkView.ApiRouter.ApiService.Utilities;

namespace WorkviewApiRouter.ApiService.RouterApi
{
    public static class WorkViewObjectAPI
    {
        public static WebApplication MapObjectAPI(this WebApplication app, string? prefix)
        {
            var mapGroup = app.MapGroup(prefix?.SanitizeUrl() ?? $"/{AppVariables.Application?.SanitizeUrl()}");
            app.MapPost("/{wvclass}", (string wvclass) =>
            {
                Console.WriteLine("😘");
            })
            .WithName("Object Endpoint")
            .WithOpenApi();
            return app;
        }
    }
}
