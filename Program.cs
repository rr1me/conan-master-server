using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.ModelBinder;
using conan_master_server.ServerLogic;
using conan_master_server.Tickets;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(x =>
{
    var connectionString = builder.Configuration.GetSection("ConnectionString").Value;
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    x.UseMySql(connectionString, serverVersion);
});

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(x =>
{
    x.AddFile("app.log", x=>x.MinLevel = LogLevel.Debug);
});
builder.WebHost.ConfigureLogging(x =>
{
    x.AddConsole();
    x
        .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning)
        .AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.Debug)
        .AddFilter("Microsoft.EntityFrameworkCore.ChangeTracking", LogLevel.Debug);
});

builder.Services.AddHttpClient();

builder.Services.AddSingleton<CleanerOrchestrator>();
builder.Services.AddTransient<ServerCleaner>();
builder.Services.AddSingleton<ServerHandler>();
builder.Services.AddSingleton<SocketHandler>();
builder.Services.AddTransient<RandomGenerator>();
builder.Services.AddScoped<ResponseWrapper>();

builder.Services.AddSingleton<TicketHandler>();
builder.Services.AddSingleton<RequestHandler>();

builder.Services.AddSingleton<PlayerData>();

builder.Services.AddTransient<TreatmentAssignment>();
builder.Services.AddTransient<InfoResultPayload>();
builder.Services.AddTransient<SettingsForUser>();
builder.Services.AddTransient<LoginData>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("http://localhost:5030/swagger/v1/swagger.json", "conan-master-server v1"));
}

app.MapControllers();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(webSocketOptions);

var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
db.Database.EnsureCreated();
db.SaveChanges();

app.Use(async (context, next) =>
{
    var logger = app.Logger;

    var requestPath = context.Request.Path;
    var requestMethod = context.Request.Method;
    logger.LogInformation($"Starting: {requestPath} | Method: {requestMethod} _______________________________________________________________");
    logger.LogInformation($"Headers: {string.Join(" | ", context.Request.Headers.ToArray())} _______________________________________________________________");
    
    await next.Invoke();

    var responseStatusCode = context.Response.StatusCode;
    logger.LogInformation($"Ending: {requestPath} | Code: {responseStatusCode} __________________________________________________________________" );
});

app.Run();