using System.Collections.Generic;
using System.Linq;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Extensions
{
	public static class TestCasesExtensions
	{
		/// <summary>
		/// Create a new sorted instance of this IEnumerable with test cases by comparing the Key of 
		/// unassigned test cases to the list of prioritized keys. The first element in prioritization has
		/// highest priority. Any Key that is not found in prioritizedKeys will not be included.
		/// </summary>
		/// <param name="testCases">Test cases to sort.</param>
		/// <param name="prioritizedKeys">Prioritization order, most important first, will only include test cases with these keys.</param>
		/// <returns>A new IEnumerable sorted according to prioritization list.</returns>
		public static IEnumerable<TestCase> Prioritized(this IEnumerable<TestCase> testCases, IEnumerable<string> prioritizedKeys)
		{
			var availablePrioritized = new List<TestCase>();
			var availableTestCases = testCases.Where(tc => tc.AssignedTo is null);

			foreach (var wantedTestCaseKey in prioritizedKeys)
			{
				var available = availableTestCases.FirstOrDefault(tc => tc.Key == wantedTestCaseKey);

				if (available is not null)
				{
					availablePrioritized.Add(available);
				}
			}

			return availablePrioritized;
		}
	}
}