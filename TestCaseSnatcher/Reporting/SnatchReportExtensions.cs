using Serilog;
using System;
using System.Linq;

namespace TestCaseSnatcher.Reporting
{
	static class SnatchReportExtensions
	{
		/// <summary>
		/// Logs the information in a snatch report.
		/// </summary>
		/// <param name="report">The snatch report to log information about.</param>
		public static void LogReport(this SnatchReport report)
		{
			var logger = Log.Logger.ForContext<SnatchReport>();
			logger.Information("---------------------------------------------------------------------");
			logger.Information("SNATCH REPORT");
			logger.Information("Numer of processed test cases: {TotalSnatchCount}", report.TotalCount);

			if (report.Successful.Any())
			{
				logger.Information("Successfully snatched {SuccessfulSnatchCount} test cases: {SuccessfulSnatches}",
					report.Successful.Count(), String.Join(", ", report.Successful.Select(tc => tc.Key)));
			}
			if (report.Failed.Any())
			{
				logger.Information("Failed to snatch {FailedSnatchCount} test cases: {FailedSnatches}"
					, report.Failed.Count(), String.Join(", ", report.Failed.Select(tc => tc.Key)));
			}
			if (report.Skipped.Any())
			{
				logger.Information("Skipped to snatch {SkippedSnatchCount} test cases: {SkippedSnatches}"
					, report.Skipped.Count(), String.Join(", ", report.Skipped.Select(tc => tc.Key)));
			}
			logger.Information("---------------------------------------------------------------------");
		}
	}
}