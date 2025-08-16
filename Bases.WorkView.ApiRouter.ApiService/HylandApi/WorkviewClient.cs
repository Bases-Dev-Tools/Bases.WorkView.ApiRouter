using Refit;
using System.Net;
using System.Runtime.CompilerServices;

public static class WorkViewService
{
    public static Token token {  get; set; }
    private static HttpClient client { get; set; }
    private static WebApplication _app { get; set; }
    public static WorkViewApplication? Application { get; set; }
    public static WorkViewClasses? Classes { get; set; }
    public static WorkViewAttributes? Attributes { get; set; }
    public static CookieContainer cookieContainer { get; set; } = new CookieContainer();
    public static IWorkviewClient wvClient => _app.Services.GetRequiredService<IWorkviewClient>();

    public static IServiceCollection AddWorkViewService(this IServiceCollection services)
    {
        services.AddRefitClient<IWorkviewClient>(settings)
        .ConfigureHttpClient(c => c.BaseAddress = AppVariables.Origin)
        .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(120))
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true
        });        

        return services;
    }
    public async static Task<WebApplication> RunWorkViewClient(this WebApplication app)
    {
        _app = app;
        var token = await wvClient.GetAuthToken(HylandAuth.Body);
        Console.WriteLine(token.AccessToken);
        var apps = await wvClient.GetWorkViewApplications($"Bearer {token.AccessToken}");        
        Application = apps.GetbyName("RapidCRM");
        if (Application == null) throw new Exception("Could not retrieve the configured WorkView Application");

        Classes = await wvClient.GetWorkViewClasses($"Bearer {token.AccessToken}", Application.Id);       
       
        foreach (var clss in Classes.Classes)
        {
            var attrs = await wvClient.GetClassAttributes($"Bearer {token.AccessToken}", clss.Id);
          
            foreach(var att in attrs.Attributes)
            {
                if(att.DataType == "Relation" && att.RelatedClassId != null)
                {
                    att.RelatedClass = Classes.GetbyId(att.RelatedClassId).SystemName;
                }
            }
            clss.Attributes = attrs.Attributes;
        }
        Application.Classes = Classes.Classes;

        return app;


    }
    public static async Task<string> GetTokenAsync()
    {
        token = await wvClient.GetAuthToken(HylandAuth.Body);
        Console.WriteLine(token.AccessToken);
        return token.AccessToken;
    }
    public static RefitSettings settings = new()
    {
        AuthorizationHeaderValueGetter = (message, cancellationToken) => GetTokenAsync()
    };

}
