using System.Collections.Generic;
using TestCaseSnatcher.Settings;

namespace TestCaseSnatcher.Tests.Data
{
	public static class Default
	{
		public static string User
			=> Settings.JiraSettings.User;

		public static string TestRunKey
			=> "TST-0006";

		public static string TestCasePrefix
			=> "TCT-";

		public static string Description
			=> "Default test run description";

		public static AppSettings Settings
			=> new()
            {
				NumberOfTestCasesToTake = 5,
				JiraSettings = new JiraSettings
				{
					User = "freer"
				},
				TestSettings = new TestSettings(),
				PrioritizedTestCases = new List<string>() { $"{TestCasePrefix}-0001",
					$"{TestCasePrefix}-0003",
					$"{TestCasePrefix}-0005",
					$"{TestCasePrefix}-0007",
					$"{TestCasePrefix}-0009",
					$"{TestCasePrefix}-0011",
					$"{TestCasePrefix}-0013",
					$"{TestCasePrefix}-0015" }
			};
	}
}