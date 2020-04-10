using Serilog;
using System;
using System.Linq;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher
{
	internal class TestRunSnatchValidator
	{
		/// <summary>
		/// Validates that a <c cref="TestRun"/> is not in status "Done", its description contains <c>descriptionFilter</c>
		/// and <c>userName</c> does not currently have any test cases assigned.
		/// </summary>
		/// <param name="testRun">The <c cref="TestRun"/> to validate.</param>
		/// <param name="userName">The username that must not have any test cases assigned already.</param>
		/// <param name="descriptionFilter">Description must contain this.</param>
		/// <returns>True if the <c cref="TestRun"/> is not "Done", contains <c>descriptionFilter</c> and
		/// <c>userName</c> does not have any issues assigned, otherwise false.</returns>
		public static bool IsValid(TestRun testRun, string userName, string descriptionFilter)
		{
			var logger = Log.Logger.ForContext<TestRunSnatchValidator>();
			return IsActive(logger, testRun) && MatchesFilter(logger, testRun, descriptionFilter)
				&& UserUnassigned(logger, testRun, userName);
		}

		/// <summary>
		/// Validates that a test run was created at least <c>minimumAgeMinutes</c> ago.
		/// If specifying 0 the validation will always succeed.
		/// </summary>
		/// <param name="testRun">The <c>TestRun</c> to validate.</param>
		/// <param name="minimumAgeMinutes">Minimum age for the <c cref="TestRun"/>, 0 will always validate successfully.</param>
		/// <returns>True if the <c cref="TestRun"/> was creates at least <c>minimumAgeMinutes</c> minutes ago, otherwise false.</returns>
		public static bool IsOldEnough(TestRun testRun, uint minimumAgeMinutes)
		{
			if (minimumAgeMinutes != 0 && DateTime.UtcNow < testRun.CreatedAt.AddMinutes(minimumAgeMinutes))
			{
				Log.Logger.ForContext<TestRunSnatchValidator>().Information("Test run is not old enough yet. It was created {CreatedAt}", testRun.CreatedAt);
				return false;
			}

			return true;
		}

		private static bool IsActive(ILogger logger, TestRun testrun)
		{
			if (testrun.Status == "Done")
			{
				logger.Information("Testrun is already finished");
				return false;
			}

			return true;
		}

		private static bool MatchesFilter(ILogger logger, TestRun testRun, string filter)
		{
			if (!testRun.Description.Contains(filter))
			{
				logger.Information("Required filter \"{DescriptionFilter}\" not found in description: {TestRunDescription}", filter, testRun.Description);
				return false;
			}

			return true;
		}

		private static bool UserUnassigned(ILogger logger, TestRun testRun, string userName)
		{
			var assignedToUser = testRun.AssignedToUser(userName);

			if (assignedToUser.Any())
			{
				logger.Information("User already has the following test cases assigned: {AlreadyAssignedTestCases}", String.Join(", ", assignedToUser));
				return false;
			}

			return true;
		}
	}
}