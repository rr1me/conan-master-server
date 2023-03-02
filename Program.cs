using conan_master_server.Additional;
using conan_master_server.Data;
using conan_master_server.ModelBinder;
using conan_master_server.ServerLogic;
using conan_master_server.Tickets;
using conan_master_server.Tokens;
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

builder.Services.AddHttpClient();

builder.Services.AddTransient<ServerCleaner>();
builder.Services.AddSingleton<ServerHandler>();
builder.Services.AddSingleton<SocketHandler>();
builder.Services.AddTransient<RandomGenerator>();
builder.Services.AddScoped<ResponseWrapper>();

builder.Services.AddSingleton<TicketHandler>();
builder.Services.AddSingleton<RequestHandler>();

builder.Services.AddSingleton<PlayerData>();
builder.Services.AddSingleton<TokenGenerator>();


builder.Services.AddTransient<TreatmentAssignment>();
builder.Services.AddTransient<InfoResultPayload>();
builder.Services.AddTransient<SettingsForUser>();
builder.Services.AddTransient<LoginData>();


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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
var cleaner = scope.ServiceProvider.GetRequiredService<ServerCleaner>();
cleaner.Scope = scope;
Task.Run(cleaner.Cleanup);

app.Run();