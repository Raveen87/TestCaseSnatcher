using System.Threading.Tasks;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Services
{
	public interface ISnatchService
	{
		/// <summary>
		/// Gets the test run with the specified key, and if all configured criteria is fulfilled, performs a snatch.
		/// </summary>
		/// <param name="testRunKey">The test run key to attempt to snatch test cases from.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		Task<SnatchStatus> AttemptSnatchAsync(string testRunKey);
	}
}