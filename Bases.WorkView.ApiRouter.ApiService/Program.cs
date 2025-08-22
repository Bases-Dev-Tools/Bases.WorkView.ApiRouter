using Microsoft.AspNetCore.OpenApi;
using Refit;
using System.Net;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args).RegisterServices();
var app = builder.Build().RegisterMiddlewares();
//app.MapWvClientApi();
app.MapObjectAPI();

app.RunWorkViewClient();

app.Run();




