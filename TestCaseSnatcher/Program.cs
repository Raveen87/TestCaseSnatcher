using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestCaseSnatcher.Settings;
using Rajven.PersistableOptions;
using TestCaseSnatcher.Jira;
using TestCaseSnatcher.Services;
using Serilog;
using TestCaseSnatcher.Jira.Services;

namespace TestCaseSnatcher
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.WriteTo.File("Logs\\TestCaseSnatcher.log", rollingInterval: RollingInterval.Year, fileSizeLimitBytes: 10 * 1024 * 1024, rollOnFileSizeLimit: true)
				.CreateLogger();

			CreateHostBuilder(args)
				.Build()
				.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureServices((hostContext, services) => {
				services.AddSingleton<ISnatchService>(provider => new SnatchService(provider.GetService<IPersistableOptions<AppSettings>>().Value, provider.GetService<ITestCaseHandler>()));
				services.AddPersistableTransient<AppSettings>(null, hostContext.Configuration.GetSection("AppSettings"));
				services.AddSingleton((provider) => TestCaseHandlerFactory.Create(provider.GetService<IPersistableOptions<AppSettings>>().Value));

				services.AddHostedService<Worker>();
			});
	}
}