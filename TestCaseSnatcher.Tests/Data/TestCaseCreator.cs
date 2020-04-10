using System.Collections.Generic;
using System.Linq;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Tests.Data
{
	public class TestCaseCreator
	{
		private int _counter;

		public TestCaseCreator()
		{
			_counter = 1;
		}

		public IEnumerable<TestCase> CreateTestCases(int number, string user = null)
			=> GenerateKeys(number)
				.Select(key => new TestCase(key, user));

		private IEnumerable<string> GenerateKeys(int number)
			=> Enumerable.Range(_counter, number)
				.Select(_ => $"{Default.TestCasePrefix}{_counter++:D4}");
	}
}