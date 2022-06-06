using Mela.AuthService.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddUserServices(builder);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();