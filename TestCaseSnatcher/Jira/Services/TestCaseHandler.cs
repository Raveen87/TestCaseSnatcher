using TestCaseSnatcher.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using TestCaseSnatcher.Settings;
using Serilog;
using TestCaseSnatcher.Jira.Json;
using TestCaseSnatcher.Jira.Models;
using TestCaseSnatcher.Jira.Extensions;

namespace TestCaseSnatcher.Jira.Services
{
	internal class TestCaseHandler : ITestCaseHandler
	{
		private readonly AppSettings _settings;
		private readonly ILogger _logger;
		private readonly IQuerier _jiraQuerier;

		public TestCaseHandler(AppSettings settings, IQuerier jiraQuerier)
		{
			_logger = Log.Logger.ForContext<TestCaseHandler>();
			_settings = settings;
			_jiraQuerier = jiraQuerier;
		}

		public async Task<TestRun?> GetTestRunAsync(string key)
		{
			var response = await _jiraQuerier.GetAsync(_settings.JiraSettings.GetTestRunAddress(key)).ConfigureAwait(false);
            LogResponse(response);

			try
			{
				return response.IsOk()
					? JsonHandler.ParseJson(JsonDocument.Parse(response.Response))
					: null;
			}
			catch (Exception e)
			{
				_logger.Warning(e, "Exception thrown when parsing Json document");
				return null;
			}
		}

		public async Task<bool> AssignTestCaseAsync(string testRunKey, string testCaseKey, string user)
		{
			var address = _settings.JiraSettings.GetTakeTestCaseAddress(testRunKey, testCaseKey);

			return (await _jiraQuerier.PutAsync(address, new AssignTo(user)).ConfigureAwait(false)).IsOk();
		}

		private static void LogResponse(JiraResponse jiraResponse)
		{
			switch (jiraResponse.ResponseType)
			{
				case ResponseType.Ok:
					break;

				case ResponseType.TimedOut:
					Log.Information("The request timed out, either you have the wrong address configured or the server is not reachable");
					break;

				case ResponseType.NotAuthorized:
					Log.Warning("You are not authorized to access the server resource, maybe your password has expired");
					break;

				case ResponseType.NotFound:
					Log.Information("The requested test run was not found, it has probably not been created yet");
					break;

				case ResponseType.OtherNonOk:
					Log.Warning("Other non-Ok response code was received from the server");
					break;

				default:
					throw new ArgumentException($"Unexpected ResponseType encountered: {jiraResponse.ResponseType}");
			}
		}
	}
}