using System.Collections.Generic;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Reporting
{
	class SnatchReportGenerator
	{
		private readonly List<TestCase> _snatchedTestCases = new();
		private readonly List<TestCase> _failedSnatches = new();
		private readonly List<TestCase> _skippedSnatches = new();

		/// <summary>
		/// Adds a test case to the list of successfully snatched test cases.
		/// </summary>
		/// <param name="snatched">The test case that was snatched.</param>
		public void AddSnatched(TestCase snatched)
			=> _snatchedTestCases.Add(snatched);

		/// <summary>
		/// Adds a test case to the list of test cases that was not successfully snatched.
		/// </summary>
		/// <param name="failed">The test case that could not be snatched.</param>
		public void AddFailed(TestCase failed)
			=> _failedSnatches.Add(failed);

		/// <summary>
		/// Adds a test case to the list of skipped test cases.
		/// </summary>
		/// <param name="skipped">The skipped test case.</param>
		public void AddSkipped(TestCase skipped)
			=> _skippedSnatches.Add(skipped);

		/// <summary>
		/// Adds multiple test cases to the list of skipped test cases.
		/// </summary>
		/// <param name="skipped">The skipped test cases.</param>
		public void AddSkipped(IEnumerable<TestCase> skipped)
			=> _skippedSnatches.AddRange(skipped);

		/// <summary>
		/// Generates the report based on previously added test cases.
		/// </summary>
		/// <returns>The generated snatch report.</returns>
		public SnatchReport Generate()
			=> SnatchReport.Create(_snatchedTestCases, _failedSnatches, _skippedSnatches);
	}
}