
using Refit;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Net;

public interface IWorkviewClient
{
    [Headers("Hyland-License-Type: QueryMetering")]
    [Post("/auth/connect/token")]
    Task<Token> GetAuthToken([Body] FormUrlEncodedContent formBody);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Post("/api/auth/diagnostics")]
    Task HealthCheck();

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/applications")]
    Task<WorkViewApplications> GetWorkViewApplications([Header("Authorization")] string auth);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/applications/{id}")]
    Task<WorkViewApplication> GetWorkViewApplication([Header("Authorization")] string auth, string id);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/applications/{id}/classes")]
    Task<WorkViewClasses> GetWorkViewClasses([Header("Authorization")] string auth, string id);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/classes/{id}")]
    Task<WorkViewClass> GetWorkViewClass([Header("Authorization")] string auth, string id);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/workview/classes/{id}/attributes")]
    Task<WorkViewAttributes> GetClassAttributes([Header("Authorization")] string auth,string id);       

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/api/onbase/core/session/disconnect")]
    Task Disconnect([Header("Authorization")] string auth, string wvclass);
}


