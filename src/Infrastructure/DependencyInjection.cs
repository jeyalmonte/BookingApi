using Application.Common.Interfaces;
using Infrastructure.BackgroundJobs;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddHttpContextAccessor()
			.AddPersistence(configuration)
			.AddServices()
			.AddMessaging(configuration)
			.AddBackgroundJobs();

		return services;
	}

	public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<InsertOutboxMessageInterceptor>();

		services.AddScoped<AppDbContextInitializer>();

		services.AddDbContext<IAppDbContext, AppDbContext>((serviceProvider, options) =>
		{
			var interceptor = serviceProvider.GetRequiredService<InsertOutboxMessageInterceptor>();

			options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), builder =>
			builder.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName))
				   .AddInterceptors(interceptor);
		});

		return services;
	}

	private static IServiceCollection AddServices(this IServiceCollection services)
	{
		services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
		services.AddTransient<IEmailService, SendgridEmailService>();

		return services;
	}

	private static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMassTransit(x =>
		{
			x.AddConsumers(typeof(DependencyInjection).Assembly);
			x.UsingRabbitMq((ctx, cfg) =>
			{
				cfg.ConfigureEndpoints(ctx);
				cfg.Host(configuration["RabbitMQ:HostName"], r =>
				{
					r.Username(configuration["RabbitMQ:UserName"]!);
					r.Password(configuration["RabbitMQ:Password"]!);
				});

				cfg.UseMessageRetry(retry =>
				{
					retry.Interval(3, TimeSpan.FromSeconds(10));
				});
			});
		});

		return services;
	}

	public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
	{
		services.AddQuartz(config =>
		{
			var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
			config
			.AddJob<ProcessOutboxMessagesJob>(jobKey)
			.AddTrigger(t =>
				t.ForJob(jobKey)
				 .WithSimpleSchedule(s =>
					s.WithInterval(TimeSpan.FromSeconds(30))
					 .RepeatForever()));

			var processFlightsJobKey = new JobKey(nameof(ProcessFlightsJob));
			config
			.AddJob<ProcessFlightsJob>(processFlightsJobKey)
			.AddTrigger(t =>
				t.ForJob(processFlightsJobKey)
				 .WithSimpleSchedule(s =>
					s.WithInterval(TimeSpan.FromMinutes(5))
					 .RepeatForever()));
		});

		services.AddQuartzHostedService(options =>
		{
			options.AwaitApplicationStarted = true;
			options.WaitForJobsToComplete = true;
		});

		return services;
	}
}
