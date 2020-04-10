using System.Collections.Generic;

namespace TestCaseSnatcher.Settings
{
	public class AppSettings
	{
		public JiraSettings JiraSettings { get; set; } = new JiraSettings();

		public TestSettings TestSettings { get; set; } = new TestSettings();

		public bool DryRun { get; set; }

		public int SleepMinutes { get; set; } = 60;

		public int SleepMs
			=> SleepMinutes * 60 * 1000;

		public int NumberOfTestCasesToTake { get; set; }

		public List<string> PrioritizedTestCases { get; set; } = new List<string>();
	}
}