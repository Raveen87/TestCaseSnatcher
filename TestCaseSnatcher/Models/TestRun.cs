using System;
using System.Collections.Generic;
using System.Linq;

namespace TestCaseSnatcher.Models
{
	public class TestRun
	{
		/// <summary>
		/// Initializes and returns a new TestRun with the specified parameters.
		/// </summary>
		/// <param name="key">The test run key.</param>
		/// <param name="description">The description for the test run.</param>
		/// <param name="testCases">The test cases the test run contains.</param>
		/// <param name="status">The status of the test run.</param>
		/// <param name="createdAt">The timestamp when the test run was created.</param>
		/// <param name="updatedAt">The timestamp when the test run was last updated.</param>
		public TestRun(string key, string description, IEnumerable<TestCase> testCases, string status, DateTime createdAt, DateTime updatedAt)
		{
			Key = key;
			Description = description;
			TestCases = testCases;
			Status = status;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
		}

		/// <summary>
		/// Gets the test run key.
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// Gets the test run description.
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Gets total number of test cases.
		/// </summary>
		public int NrTestCases
			=> TestCases.Count();

		/// <summary>
		/// Gets all test cases.
		/// </summary>
		public IEnumerable<TestCase> TestCases { get; }

		/// <summary>
		/// Gets test cases that are currently unassigned.
		/// </summary>
		public IEnumerable<TestCase> UnassignedTestCases
			=> TestCases.Where(t => t.AssignedTo is null);

		/// <summary>
		/// Gets the status of the test run.
		/// </summary>
		public string Status { get; }

		/// <summary>
		/// Gets the timestamp when this test run was created.
		/// </summary>
		public DateTime CreatedAt { get; }

		/// <summary>
		/// Gets the timestamp when this test run was last updated.
		/// </summary>
		public DateTime UpdatedAt { get; }
	}
}