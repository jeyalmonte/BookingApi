using Api;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services
	.AddPresentation()
	.AddApplication()
	.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	using var scope = app.Services.CreateScope();
	var dbContextInitialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();

	await dbContextInitialiser.InitialiseAsync();
	await dbContextInitialiser.TrySeedAsync();
}
app.Use((context, next) =>
{
	context.Response.Headers.XContentTypeOptions = "nosniff";
	context.Response.Headers.XFrameOptions = "DENY";
	return next.Invoke();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
