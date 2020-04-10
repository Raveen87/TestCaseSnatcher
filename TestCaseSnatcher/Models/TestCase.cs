namespace TestCaseSnatcher.Models
{
	public class TestCase
	{
		/// <summary>
		/// Initializes and returns a new TestCase with the specified parameters.
		/// </summary>
		/// <param name="key">The test case key.</param>
		/// <param name="assignedTo">The user this test case is assigned to, null if unassigned.</param>
		public TestCase(string key, string? assignedTo)
		{
			Key = key;
			AssignedTo = assignedTo;
		}

		/// <summary>
		/// Gets the test case key.
		/// </summary>
		public string Key { get; }

		/// <summary>
		/// Gets the user this test case is assigned to, null if unassigned.
		/// </summary>
		public string? AssignedTo { get; }

		public override string ToString()
			=> Key;
	}
}