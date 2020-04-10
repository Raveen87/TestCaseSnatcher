using System.Collections.Generic;
using System.Linq;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Reporting
{
	public sealed class SnatchReport
	{
		private SnatchReport(IEnumerable<TestCase> sucessful, IEnumerable<TestCase> failed, IEnumerable<TestCase> skipped)
		{
			Successful = sucessful;
			Failed = failed;
			Skipped = skipped;
		}

		/// <summary>
		/// Gets the test cases that was successfully snatched.
		/// </summary>
		public IEnumerable<TestCase> Successful { get; }

		/// <summary>
		/// Gets the test cases that was attempted to be snatched, but failed.
		/// </summary>
		public IEnumerable<TestCase> Failed { get; }

		/// <summary>
		/// Gets the test cases that were skipped altogether.
		/// </summary>
		public IEnumerable<TestCase> Skipped { get; }

		/// <summary>
		/// Gets total number of processed test cases.
		/// </summary>
		public int TotalCount
			=> Successful.Count() + Failed.Count() + Skipped.Count();

		/// <summary>
		/// Creates a new SnatchReport with the specified successful, failed and skipped test cases.
		/// </summary>
		/// <param name="successful">Test cases that was successfully snatched.</param>
		/// <param name="failed">Test cases that was attempted to be snatched, but failed.</param>
		/// <param name="skipped">Test cases that was skipped.</param>
		/// <returns>A new SnatchReport with the successfull, failed and skipped test cases.</returns>
		public static SnatchReport Create(IEnumerable<TestCase> successful, IEnumerable<TestCase> failed, IEnumerable<TestCase> skipped)
			=> new(successful, failed, skipped);
	}
}