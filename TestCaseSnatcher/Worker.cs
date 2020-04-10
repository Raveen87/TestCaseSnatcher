using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TestCaseSnatcher.Settings;
using Rajven.PersistableOptions;
using TestCaseSnatcher.Services;
using Serilog;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher
{
	public class Worker : BackgroundService
	{
		private readonly IPersistableOptions<AppSettings> _settings;
		private readonly ILogger _logger;
		private readonly ISnatchService _snatchService;

		public Worker(IPersistableOptions<AppSettings> settings, ISnatchService snatchService)
		{
			_settings = settings;
			_logger = Log.Logger.ForContext<Worker>();
			_snatchService = snatchService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.Information("Starting up with config: {@Config}", _settings.Value);

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.Information("Worker running at: {RunTime}", DateTimeOffset.Now);
				await DoWork().ConfigureAwait(false);

				_logger.Information("Sleeping for {SleepTime} minutes", _settings.Value.SleepMinutes);
				await Task.Delay(_settings.Value.SleepMs, stoppingToken).ConfigureAwait(false);
			}
		}

		private async Task DoWork()
		{
			var testRunKey = _settings.Value.TestSettings.ActiveTestRun;
			var status = await _snatchService.AttemptSnatchAsync(testRunKey).ConfigureAwait(false);

			if (status == SnatchStatus.Waiting)
			{
				_logger.Information("Sleeping and trying again later...");
			}
			else if (status == SnatchStatus.NotAvailable || status == SnatchStatus.Success)
			{
				_logger.Information("Going to next test run");
				_settings.Persist(settings => ++settings.TestSettings.ActiveTestRunId);
			}
		}
	}
}