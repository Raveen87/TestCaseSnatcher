using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Rajven.PersistableOptions
{
	public static class ServiceCollectionExtensions
	{
		private const string DEFAULT_SETTINGS_PATH = "appsettings.json";

		/// <summary>
		/// Adds an IPersistableOptions<T> instance as a transient service to the service collection.
		/// </summary>
		/// <typeparam name="T">The type of the configuration.</typeparam>
		/// <param name="services">The service collection.</param>
		/// <param name="settingsFilePath">File path to the settings file, defaults to appsettings.json in the working directory.</param>
		/// <param name="section">The configuration section that makes up the T instance, defaults to null, i.e. the whole file</param>
		/// <returns>A reference to this IServiceCollection after the operation has completed.</returns>
		public static IServiceCollection AddPersistableTransient<T>(this IServiceCollection services,
			string settingsFilePath = null,
			IConfigurationSection section = null)
			where T : class, new()
			=> services.Configure<T>(section)
				.AddTransient((provider) => ServiceCreator<T>(settingsFilePath, section).Invoke(provider));

		/// <summary>
		/// Adds an IPersistableOptions<T> instance as a singleton service to the service collection.
		/// </summary>
		/// <typeparam name="T">The type of the configuration.</typeparam>
		/// <param name="services">The service collection.</param>
		/// <param name="settingsFilePath">File path to the settings file, defaults to appsettings.json in the working directory.</param>
		/// <param name="section">The configuration section that makes up the T instance, defaults to null, i.e. the whole file</param>
		/// <returns>A reference to this IServiceCollection after the operation has completed.</returns>
		public static IServiceCollection AddPersistableSingleton<T>(this IServiceCollection services,
			string settingsFilePath = null,
			IConfigurationSection section = null)
			where T : class, new()
			=> services.Configure<T>(section)
				.AddSingleton((provider) => ServiceCreator<T>(settingsFilePath, section).Invoke(provider));

		private static Func<IServiceProvider, IPersistableOptions<T>> ServiceCreator<T>(string settingsFilePath, IConfigurationSection section)
			where T : class, new()
			=> new Func<IServiceProvider, IPersistableOptions<T>>((provider) =>
			{
				var optionsMonitor = provider.GetService<IOptionsMonitor<T>>();

				return new PersistableOptions<T>(optionsMonitor, settingsFilePath ?? DEFAULT_SETTINGS_PATH, section?.Key);
			});
	}
}