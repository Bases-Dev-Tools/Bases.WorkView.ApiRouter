using Microsoft.Extensions.Http.Logging;
using Refit;
using System.Net;
using System.Runtime.CompilerServices;
using Bases.WorkView.ApiRouter.ApiService.AssemblyGenerator;
public static class WorkViewService
{
    public static Token Token {  get; set; }
    private static HttpClient client { get; set; }
    private static WebApplication _app { get; set; }
    public static CookieContainer cookieContainer { get; set; } = new CookieContainer();
    public static IWorkviewClient wvClient => _app.Services.GetRequiredService<IWorkviewClient>();
    public static IServiceCollection AddWorkViewService(this IServiceCollection services)
    {
        services.AddRefitClient<IWorkviewClient>(settings)
        .ConfigureHttpClient(c => c.BaseAddress = AppVariables.Origin)
        .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(180))
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true
        });
        //.AddLogger((logger) =>
        //{
        //    return new HttpClientLogger();
        //});

        return services;
    }
    public async static Task<WebApplication> RunWorkViewClient(this WebApplication app)
    {
        _app = app;
        try
        {
            
            var token = await wvClient.GetAuthToken(HylandAuth.Body);
            if (!token.IsSuccessful)
                throw token.Error;
            Token = token.Content;
            var appRes = await wvClient.GetWorkViewApplications(Token.AuthHeader);
            if (!appRes.IsSuccessful)
                throw appRes.Error;
            WorkViewCache.Application = appRes.Content.GetbyName("RapidCRM");
            WorkViewCache.CustomAssembly = new AssemblyGenerator(WorkViewCache.Application);
            if (WorkViewCache.Application == null) throw new Exception("Could not retrieve the configured WorkView Application");
            var classRes = await wvClient.GetWorkViewClasses(Token.AuthHeader, WorkViewCache.Application.Id);
            if (classRes.IsSuccessful)
                WorkViewCache.Classes = classRes.Content;


            foreach (var clss in WorkViewCache.Classes.Items)
            {

                var attRes = await wvClient.GetClassAttributes(Token.AuthHeader, clss.Id);
                foreach (var att in attRes.Content.Items)
                {
                    if (att.DataType == "Relation" && att.RelatedClassId != null)
                    {
                        att.RelatedClass = WorkViewCache.Classes.GetbyId(att.RelatedClassId).SystemName;
                    }
                }
                clss.Attributes = attRes.Content.Items;
                clss.ClassType = WorkViewCache.CustomAssembly.CreateType(clss);
            }
            WorkViewCache.Application.Classes = WorkViewCache.Classes.Items;
            WorkViewCache.CustomAssembly.SaveDll();
            
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return app;
    }
    public static async Task<string> GetTokenAsync()
    {
        var tokenRes = await wvClient.GetAuthToken(HylandAuth.Body);
        if(tokenRes.IsSuccessful)
            return tokenRes.Content.AccessToken;
        else
            throw tokenRes.Error;
    }
    public static RefitSettings settings = new()
    {
        AuthorizationHeaderValueGetter = (message, cancellationToken) => GetTokenAsync()
    };

}

//TODO: Implement Logging
public class HttpClientLogger : IHttpClientLogger
{
    public void LogRequestFailed(object? context, HttpRequestMessage request, HttpResponseMessage? response, Exception exception, TimeSpan elapsed)
    {
        return;
    }

    public object? LogRequestStart(HttpRequestMessage request)
    {
        return null;
    }

    public void LogRequestStop(object? context, HttpRequestMessage request, HttpResponseMessage response, TimeSpan elapsed)
    {
        return;
    }
}