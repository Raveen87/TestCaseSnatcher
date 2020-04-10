using Rajven.ComExt.EnumeratorExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCaseSnatcher.Extensions;
using TestCaseSnatcher.Models;
using TestCaseSnatcher.Reporting;

namespace TestCaseSnatcher.Services
{
	/// <summary>
	/// Helper class to perform the snatching of a test case. Kept in a separate class like this
	/// to make unit testing easier and more separate from the other services.
	/// </summary>
	public class SnatchHelper
	{
		/// <summary>
		/// From an enumerable of available test cases, goes through them in prioritized order,
		/// and snatches a specified maximum number of available test cases.
		/// </summary>
		/// <param name="testCases">All test cases.</param>
		/// <param name="prioritizedOrder">Prioritization order, most important first, will only consider test cases with these keys.</param>
		/// <param name="snatchAction">The action to perform to snatch a test case.</param>
		/// <param name="takeMax">Maximum number of test cases to snatch.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public static async Task<SnatchReport> SnatchTestCasesAsync(IEnumerable<TestCase> testCases,
			IEnumerable<string> prioritizedOrder, Func<TestCase, Task<bool>> snatchAction, int takeMax)
		{
			int takenTestCases = 0;
			var snatchReportBuilder = new SnatchReportGenerator();
			var enumerator = testCases.Prioritized(prioritizedOrder).GetEnumerator();

			while (takenTestCases++ < takeMax && enumerator.MoveNext())
			{
				if (await snatchAction(enumerator.Current).ConfigureAwait(false))
				{
					snatchReportBuilder.AddSnatched(enumerator.Current);
				}
				else
				{
					snatchReportBuilder.AddFailed(enumerator.Current);
				}
			}

			snatchReportBuilder.AddSkipped(enumerator.GetRemaining());

			return snatchReportBuilder.Generate();
		}
	}
}