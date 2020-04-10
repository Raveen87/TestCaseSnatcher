using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using TestCaseSnatcher.Extensions;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Reporting
{
	static class TestRunExtensions
	{
		/// <summary>
		/// Logs information about a test run that was returned.
		/// </summary>
		/// <param name="testRun">The test run to log information about.</param>
		/// <param name="prioritizedOrder">The prioritization order.</param>
		/// <param name="takeMax">Maximum number of test cases to take.</param>
		public static void LogInfo(this TestRun testRun, IEnumerable<string> prioritizedOrder, int takeMax)
		{
			var logger = Log.Logger.ForContext<TestRun>();
			var total = testRun.TestCases.Count();
			IEnumerable<TestCase> unassigned = testRun.TestCases.Where(tc => tc.AssignedTo is null);
			var snatchOrder = testRun.TestCases.Prioritized(prioritizedOrder);
			var nonExisting = prioritizedOrder.Except(testRun.TestCases.Select(tc => tc.Key));
			var duplicates = prioritizedOrder.GroupBy(elem => elem).Where(grp => grp.Count() > 1).Select(duplicate => duplicate.Key);

			if (nonExisting.Any())
			{
				logger.Warning("{NonExistingTestCasesCount} prioritized test cases were not found in the test run: {NonExistingTestCases}",
					nonExisting.Count(), String.Join(", ", nonExisting));
			}

			if (duplicates.Any())
			{
				logger.Warning("There are {DuplicatedPrioritizedCount} duplicates in the prioritized test cases: {DuplicatedPrioritized}",
					duplicates.Count(), String.Join(", ", duplicates));
			}

			logger.Information("{UnassignedTestCases}/{TotalTestCases} test cases are currently unassigned", unassigned.Count(), total);
			logger.Information("There are {PrioritizedCount} test cases configured as prioritized, and {TakeMax} confiugred as maximum to take",
				prioritizedOrder.Count(), takeMax);
			logger.Information("Will take maximum {FunctionalTakeMax} test cases", Math.Min(prioritizedOrder.Count(), takeMax));
			logger.Information("Of available test cases there are {AvailablePrioritzedCount} prioritzed: {AvailablePrioritized}",
				snatchOrder.Count(), String.Join(", ", snatchOrder));
		}
	}
}