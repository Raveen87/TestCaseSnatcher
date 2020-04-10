using System;

namespace TestCaseSnatcher.Settings
{
	public class TestSettings
	{
		public string DescriptionFilter { get; set; } = String.Empty;

		public uint MinimumAgeMinutes { get; set; } = 0;

		public string TestRunPrefix { get; set; } = String.Empty;

		public int ActiveTestRunId { get; set; }

		public string ActiveTestRun { get => $"{TestRunPrefix}{ActiveTestRunId}"; }
	}
}