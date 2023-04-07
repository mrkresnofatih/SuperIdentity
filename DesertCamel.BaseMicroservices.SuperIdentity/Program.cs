using DesertCamel.BaseMicroservices.SuperIdentity;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);

app.Run();