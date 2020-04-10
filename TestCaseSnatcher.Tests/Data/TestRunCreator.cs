using System;
using System.Collections.Generic;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Tests.Data
{
	public class TestRunCreator
	{
		private readonly string _status;

		public TestRunCreator(string status)
		{
			_status = status;
		}

		public TestRun WithUnassignedTestCases()
			=> CreateTestRun();

		public TestRun WithCreatedTime(DateTime createdAt)
			=> CreateTestRun(createdAt: createdAt);

		public TestRun WithTestCases(IEnumerable<TestCase> testCases)
			=> CreateTestRun(testCases: testCases);

		private TestRun CreateTestRun(string description = null, string user = null, IEnumerable<TestCase> testCases = null,
			int numberOfTestCases = 20, DateTime? createdAt = null)
		{
			var testRunKey = Default.TestRunKey;
			var desc = description ?? Default.Description;
			var tests = testCases ?? TestData.CreateTestCases().CreateTestCases(numberOfTestCases, user);
			var created = createdAt ?? DateTime.Now.AddHours(-1);

			return new TestRun(testRunKey, desc, tests, _status, created, DateTime.Now);
		}
	}
}