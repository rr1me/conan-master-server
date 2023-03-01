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

builder.Services.AddSingleton<ServerHandler>();
builder.Services.AddSingleton<SocketHandler>();
builder.Services.AddTransient<RandomGenerator>();
builder.Services.AddScoped<ResponseWrapper>();

builder.Services.AddSingleton<TicketHandler>();
builder.Services.AddSingleton<RequestHandler>();

builder.Services.AddSingleton<PlayerData>();
builder.Services.AddSingleton<TokenGenerator>();


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();

db.Database.EnsureCreated();
db.SaveChanges();

app.Run();