using System.Text.Json.Serialization;
using Aamva.Configuration;
using Aamva.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCFConfiguration();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDldvService, DldvService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

var app = builder.Build();

app.UseMetricServer();
app.UseHttpMetrics();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace Aamva
{
    public partial class Program { }
}