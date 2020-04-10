using Serilog;
using System.Threading.Tasks;
using TestCaseSnatcher.Jira.Services;
using TestCaseSnatcher.Models;
using TestCaseSnatcher.Reporting;
using TestCaseSnatcher.Settings;

namespace TestCaseSnatcher.Services
{
	public class SnatchService : ISnatchService
	{
		private readonly ILogger _logger;
		private readonly AppSettings _settings;
		private readonly ITestCaseHandler _jiraTestCaseHandler;

		public SnatchService(AppSettings settings, ITestCaseHandler jiraTestCaseHandler)
		{
			_logger = Log.Logger.ForContext<SnatchService>();
			_settings = settings;
			_jiraTestCaseHandler = jiraTestCaseHandler;
		}

		public async Task<SnatchStatus> AttemptSnatchAsync(string testRunKey)
		{
			var testRun = await _jiraTestCaseHandler.GetTestRunAsync(testRunKey).ConfigureAwait(false);

			if (testRun is null)
			{
				return SnatchStatus.Error;
			}
			else if (!TestRunSnatchValidator.IsValid(testRun, _settings.JiraSettings.User,
				_settings.TestSettings.DescriptionFilter))
			{
				return SnatchStatus.NotAvailable;
			}
			else if (!TestRunSnatchValidator.IsOldEnough(testRun, _settings.TestSettings.MinimumAgeMinutes))
			{
				return SnatchStatus.Waiting;
			}

			var takeMax = _settings.NumberOfTestCasesToTake;

			if (_settings.DryRun)
			{
				_logger.Information("Dry run enabled, won't actually snatch any test cases, all test cases are simulated to be successfully snatched");
			}

			testRun.LogInfo(_settings.PrioritizedTestCases, takeMax);

			var report = await SnatchHelper.SnatchTestCasesAsync(testRun.TestCases, _settings.PrioritizedTestCases,
				(testCase) => GetSnatchAction(testRun.Key, testCase), takeMax)
				.ConfigureAwait(false);

			report.LogReport();
			return SnatchStatus.Success;
		}

		private Task<bool> GetSnatchAction(string testRunKey, TestCase testCase)
			=> !_settings.DryRun
				? _jiraTestCaseHandler.AssignTestCaseAsync(testRunKey, testCase.Key, _settings.JiraSettings.User)
				: Task.FromResult(true);
	}
}