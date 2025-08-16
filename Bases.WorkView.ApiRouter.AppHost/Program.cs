
var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var apiService = builder.AddProject<Projects.Bases_WorkView_ApiRouter_ApiService>("apiservice")
    .WithExternalHttpEndpoints();


builder.Build().Run();
