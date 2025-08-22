
using Refit;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Net;
using System.Runtime.CompilerServices;

public static class WorkViewClientApi
{
    public static WebApplication MapWvClientApi(this WebApplication app)
    {
        var mapGroup = app.MapGroup($"/api");
        mapGroup.MapGet("/application", () =>
        {
            return WorkViewCache.Application;
        })
        .WithName("Get Application")
        .WithDescription("Retrieve information about the WorkView Application")
        .WithOpenApi();
        mapGroup.MapGet("/classes", () =>
        {
            return WorkViewCache.Classes;
        })
        .WithName("Get WorkView Classes")
        .WithDescription("Retrieve a list of classes used in the application.")
        .WithOpenApi();
        mapGroup.MapGet("/classes/{wvclass}", (string wvclass) =>
        {
            return WorkViewCache.Find<WorkViewClass>(wvclass);
        })
        .WithName("Get WorkView Class")
        .WithDescription("Retrieve information about a particular class.")
        .WithOpenApi();
        mapGroup.MapGet("/classes/{wvclass}/attributes", (string wvclass) =>
        {
            var clss = WorkViewCache.Find<WorkViewClass>(wvclass);
            return clss.Attributes;
        })
        .WithName("Get Attributes for a class.")
        .WithDescription("Retrieve attributes for a class.")
        .WithOpenApi();
        return app;
    }
}

public interface IWorkviewClient
{
    [Headers("Hyland-License-Type: QueryMetering")]
    [Post("/auth/connect/token")]
    Task<ApiResponse<Token>> GetAuthToken([Body] FormUrlEncodedContent formBody);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Post("/api/auth/diagnostics")]
    Task HealthCheck();

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/core/session/disconnect")]
    Task Disconnect([Header("Authorization")] string auth, string wvclass);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/applications")]
    Task<ApiResponse<WorkViewApplications>> GetWorkViewApplications([Header("Authorization")] string auth);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/applications/{id}")]
    Task<ApiResponse<WorkViewApplication>> GetWorkViewApplication([Header("Authorization")] string auth, string id);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/applications/{id}/classes")]
    Task<ApiResponse<WorkViewClasses>> GetWorkViewClasses([Header("Authorization")] string auth, string id);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/classes/{id}")]
    Task<ApiResponse<WorkViewClass>> GetWorkViewClass([Header("Authorization")] string auth, string id);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/classes/{id}/attributes")]
    Task<ApiResponse<WorkViewAttributes>> GetClassAttributes([Header("Authorization")] string auth, string id);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview//datasets/{dataSetId}")]
    Task<ApiResponse<WorkViewDataSet>> GetAttributeDataSet([Header("Authorization")] string auth, string dataSetId);
    
    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/datasets/{dataSetId}/values")]
    Task<ApiResponse<WorkViewDataSetValues>> GetDataSetValues([Header("Authorization")] string auth, string dataSetId, string? parentValue = null);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/objects/{key}")]
    Task<ApiResponse<WorkViewObject?>> GetObject([Header("Authorization")] string auth, string key);
    
}


