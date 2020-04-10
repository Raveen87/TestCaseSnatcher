namespace TestCaseSnatcher.Tests.Data
{
	public static class TestData
	{
		private static readonly TestCaseCreator _testCaseCreator = new TestCaseCreator();

		public static TestRunCreator CreateTestRun(string status = "In Progress")
			=> new TestRunCreator(status);

		public static TestCaseCreator CreateTestCases()
			=> _testCaseCreator;
	}
}