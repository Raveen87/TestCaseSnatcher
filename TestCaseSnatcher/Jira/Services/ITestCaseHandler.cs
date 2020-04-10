using System.Threading.Tasks;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Jira.Services
{
	public interface ITestCaseHandler
	{
        /// <summary>
        /// Gets the test run with the specified testRunKey.
        /// </summary>
        /// <param name="key">The test run key of the test run to get.</param>
        /// <returns>The test run with the specified key, or null if not found or some error occurred.</returns>
        Task<TestRun?> GetTestRunAsync(string key);

		/// <summary>
		/// Assigns a test case in a test run to a user.
		/// </summary>
		/// <param name="testRunKey">The key of the test run in which to assign a test case.</param>
		/// <param name="testCaseKey">The key of the test case to take.</param>
		/// <param name="user">The username to assign the test case to.</param>
		/// <returns>True if the test case was successfully assigned, otherwise false.</returns>
		Task<bool> AssignTestCaseAsync(string testRunKey, string testCaseKey, string user);
	}
}