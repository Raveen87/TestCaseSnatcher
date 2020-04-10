using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TestCaseSnatcher.Models;

namespace TestCaseSnatcher.Jira.Json
{
	/// <summary>
	/// Handles parsing of the returned JSON.
	/// </summary>
	internal static class JsonHandler
	{
		/// <summary>
		/// Parses the TestRun from a JsonDocument that was returned from the Jira plugin.
		/// </summary>
		/// <param name="jsonDocument">The returned JSON.</param>
		/// <returns>The parsed TestRun.</returns>
		public static TestRun ParseJson(JsonDocument jsonDocument)
		{
			var description = GetDescription(jsonDocument);
			var key = GetKey(jsonDocument);
			var status = GetStatus(jsonDocument);
			var testCases = GetTestCases(jsonDocument);
			var createdAt = GetCreatedAt(jsonDocument);
			var updatedAt = GetUpdatedAt(jsonDocument);

			return new TestRun(key, description, testCases, status, createdAt, updatedAt);
		}

		private static string GetDescription(JsonDocument jsonDocument)
			=> jsonDocument.RootElement
			.GetProperty("description")
			.GetString() ?? String.Empty;

		private static string GetKey(JsonDocument jsonDocument)
			=> jsonDocument.RootElement
			.GetProperty("key")
			.GetString() ?? String.Empty;

		private static string GetStatus(JsonDocument jsonDocument)
			=> jsonDocument.RootElement
			.GetProperty("status")
			.GetString() ?? String.Empty;

		private static DateTime GetCreatedAt(JsonDocument jsonDocument)
			=> jsonDocument.RootElement
			.GetProperty("createdOn")
			.GetDateTime();

		private static DateTime GetUpdatedAt(JsonDocument jsonDocument)
			=> jsonDocument.RootElement
			.GetProperty("updatedOn")
			.GetDateTime();

		private static IEnumerable<TestCase> GetTestCases(JsonDocument jsonDocument)
			=> jsonDocument.RootElement
			.GetProperty("items")
			.EnumerateArray()
			.Select(i => CreateTestCase(i));

		private static TestCase CreateTestCase(JsonElement elem)
			=> new(elem.GetProperty("testCaseKey").GetString() ?? String.Empty, GetAssignedTo(elem));

		private static string? GetAssignedTo(JsonElement elem)
		{
			var hasAssignedTo = elem.TryGetProperty("assignedTo", out var assignedToJson);
			return hasAssignedTo ? assignedToJson.GetString() : null;
		}
	}
}