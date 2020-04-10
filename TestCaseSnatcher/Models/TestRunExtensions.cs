using System;
using System.Collections.Generic;
using System.Linq;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher
{
	internal static class TestRunExtensions
	{
		/// <summary>
		/// Gets all test cases assigned to the specified user.
		/// </summary>
		/// <param name="testRun">The test run with test cases to process.</param>
		/// <param name="userName">The username for which to return test cases assigned to.</param>
		/// <returns>All test cases assigned to the specified user.</returns>
		public static IEnumerable<TestCase> AssignedToUser(this TestRun testRun, string userName)
			=> testRun.TestCases
			.Where(tc => tc.AssignedTo?.Equals(userName, StringComparison.InvariantCultureIgnoreCase) == true);
	}
}