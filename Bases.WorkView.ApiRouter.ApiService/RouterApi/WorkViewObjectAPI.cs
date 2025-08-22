
using Microsoft.AspNetCore.Mvc;
using Refit;

public static class WorkViewObjectApi
{
    public static WebApplication MapObjectAPI(this WebApplication app, string? prefix = null)
    {
        var mapGroup = app.MapGroup(prefix?.SanitizeUrl() ?? $"/{AppVariables.Application?.SanitizeUrl()}");

        mapGroup.MapGet("/{wvclass}/schema", async ([FromRoute] string wvclass) =>
        {
            var wvclss = WorkViewCache.Find<WorkViewClass>(wvclass);
            var obj = WorkViewCache.CustomAssembly.Assembly.CreateInstance(wvclss.ClassType.Name);

            return obj;
        })
        .WithName("Get Class Schema")
        .WithDescription("Gets a description of the class.")
        .WithOpenApi();
        //mapGroup.MapGet("/{wvclass}/{key}", async ([FromRoute] string wvclass, [FromRoute] string key) =>
        //{
        //    var c = WorkViewCache.Find<WorkViewClass>(wvclass);
        //    var o = await WorkViewService.wvClient.GetObject(WorkViewService.Token.AccessToken, key) ?? null;
        //    if (o == null)
        //    {
        //        return Results.NotFound();
        //    }
        //    return Results.Json(o);
        //})
        //.WithName("Retrieve an object of the class that's being passed in the url.")
        //.WithOpenApi();

        mapGroup.MapPost("/{wvclass}", async (string wvclass, [FromBody] PostObjectDto body) =>
        {
            
        })
        .WithName("Create An Object")
        .WithDescription("Create an object of the class. Use '/schema' to get the class structure.")
        .WithOpenApi();
        return app;
    }
}
