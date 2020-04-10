namespace TestCaseSnatcher.Models
{
	internal class AssignTo
	{
		/// <summary>
		/// DTO for serializing an update where a test case is assigned to a user.
		/// </summary>
		/// <param name="user">The user to assign the test case to.</param>
		public AssignTo(string user)
		{
			AssignedTo = user;
		}

		/// <summary>
		/// Gets the user this test case will be assigned to.
		/// </summary>
		public string AssignedTo { get; set; }
	}
}