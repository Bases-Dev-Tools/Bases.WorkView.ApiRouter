using Refit;
using System.ComponentModel.DataAnnotations;

public interface IRouterApi
{
    /// <summary>
    /// Create a new item of a specific class
    /// </summary>
    /// <param name="auth"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    //C in CRUD
    [Headers("Hyland-License-Type: QueryMetering")]
    [Post("/{wvclass}")]
    Task<PostWorkViewObjectRequest> PostWorkViewObject(string wvclass, [Body] PostWorkViewObjectRequest body);

    //R in CRUD
    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/{wvclass}/{id}")]
    Task<GetWorkViewObjectRequest> GetWorkViewObject(string id, [Query]bool hydrate);

    [Headers("Hyland-License-Type: QueryMetering")]
    [Get("/{wvclass}/{id}")]
    Task<GetWorkViewObjectRequest> GetWorkViewObject(string id, [Query] bool hydrate, [Query(CollectionFormat.Multi)] string[] searchParams);

    //U in CRUD
    [Headers("Hyland-License-Type: QueryMetering")]
    [Put("/{wvclass}/{id}")]
    Task<UpdateWorkViewObjectRequest> UpdateWorkViewObject(string id, [Body] UpdateWorkViewObjectRequest body);

    //D in Crub
    [Headers("Hyland-License-Type: QueryMetering")]
    [Delete("/{wvclass}/{id}")]
    Task DeleteWorkViewObject(string id, [Query]bool deleteChildren = false);
}