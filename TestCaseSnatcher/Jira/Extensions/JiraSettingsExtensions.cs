using Flurl;
using TestCaseSnatcher.Settings;

namespace TestCaseSnatcher.Jira.Extensions
{
	internal static class JiraSettingsExtensions
	{
		/// <summary>
		/// Gets the HTTP address used to snatch test cases.
		/// </summary>
		/// <param name="jiraSettings">This Jira settings with configured values to use.</param>
		/// <param name="testRunKey">The key to the test run.</param>
		/// <param name="testCaseKey">The key of the test case to snatch.</param>
		/// <returns>The HTTP address of a test case that can be snatched.</returns>
		public static string GetTakeTestCaseAddress(this JiraSettings jiraSettings, string testRunKey, string testCaseKey)
			=> Url.Combine(jiraSettings.GetTestRunAddress(testRunKey), $"/testcase/{testCaseKey}/testresult");

		/// <summary>
		/// Gets the HTTP address of a test run.
		/// </summary>
		/// <param name="jiraSettings">This Jira settings with configured values to use.</param>
		/// <param name="testRunKey">The key to the test run.</param>
		/// <returns>The HTTP address of a test run.</returns>
		public static string GetTestRunAddress(this JiraSettings jiraSettings, string testRunKey)
			=> Url.Combine(jiraSettings.GetApiUrl(), testRunKey);

		private static string GetApiUrl(this JiraSettings jiraSettings)
			=> Url.Combine(jiraSettings.BaseUrl, "/rest/atm/1.0/testrun");
	}
}