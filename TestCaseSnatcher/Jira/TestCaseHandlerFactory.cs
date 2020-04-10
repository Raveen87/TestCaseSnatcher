using TestCaseSnatcher.Jira.Services;
using TestCaseSnatcher.Settings;

namespace TestCaseSnatcher.Jira
{
	public static class TestCaseHandlerFactory
	{
		/// <summary>
		/// Creates a new ITestCaseHandler based on the specified settings.
		/// </summary>
		/// <param name="settings">The settings to use.</param>
		/// <returns>A new ITestCaseHandler created based on the specified settings.</returns>
		public static ITestCaseHandler Create(AppSettings settings)
			=> new TestCaseHandler(settings, new Querier(settings.JiraSettings.User, settings.JiraSettings.Password));
	}
}