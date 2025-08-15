using DefenSys.Application.Services;
using DefenSys.Application.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Configure the default HttpClient to not use any system proxy.
builder.Services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName)
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            UseProxy = false
        };
    });

#region DI
// Register the custom service for dependency injection.
// We use AddScoped, which creates one instance per HTTP request.
builder.Services.AddScoped<ISqlInjectionService, SqlInjectionService>();
builder.Services.AddScoped<IXssScannerService, XssScannerService>();
builder.Services.AddScoped<ICommandInjectionScannerService, CommandInjectionScannerService>();
#endregion
// --- 1. Add CORS services to the container ---
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // IMPORTANT: Replace this with your Angular app's URL.
                          // The port might be different on your machine.
                          policy.WithOrigins("https://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
