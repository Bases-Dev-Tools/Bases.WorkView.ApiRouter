public static class Configuration
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.Services.AddProblemDetails();
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddWorkViewService();
        return builder;
    }
 
    public static WebApplication RegisterMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapSwagger();
        }
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapDefaultEndpoints();
        return app;
    }
}

public static class AppVariables
{
    public static string? Name => System.Environment.GetEnvironmentVariable("ONBASE_ENVIRONMENT");
    public static Uri? Origin => new Uri(System.Environment.GetEnvironmentVariable("ORIGIN"));
    public static string? IdpUri => System.Environment.GetEnvironmentVariable("IDP");
    public static string? ApiUri => System.Environment.GetEnvironmentVariable("API");
    public static string? Grant => System.Environment.GetEnvironmentVariable("GRANT_TYPE");
    public static string? Scope => System.Environment.GetEnvironmentVariable("SCOPE");
    public static string? ClientId => System.Environment.GetEnvironmentVariable("CLIENT_ID");
    public static string? Secret => System.Environment.GetEnvironmentVariable("CLIENT_SECRET");
    public static string? Tenant => System.Environment.GetEnvironmentVariable("TENANT");
    public static string? Username => System.Environment.GetEnvironmentVariable("USERNAME");
    public static string? Password => System.Environment.GetEnvironmentVariable("PASSWORD");
    public static string? Application => System.Environment.GetEnvironmentVariable("WORKVIEWAPP");
}