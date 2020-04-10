using System;

namespace TestCaseSnatcher.Jira.Models
{
	public enum ResponseType { Ok, TimedOut, NotAuthorized, NotFound, OtherNonOk }

	/// <summary>
	/// Represents a response from Jira. Can be either Ok with a content, or in an
	/// error state with no content.
	/// </summary>
	public struct JiraResponse
	{
		/// <summary>
		/// Initializes and returns a new JiraResponse with the specified response type and response.
		/// </summary>
		/// <param name="responseType">The ResponseType, either Ok or any of the specified errors.</param>
		/// <param name="response">The response, only specified if ResponseType is Ok.</param>
		public JiraResponse(ResponseType responseType, string response = "")
		{
			ResponseType = responseType;
			Response = response;
		}

		/// <summary>
		/// Initializes and returns a JiraResponse in the Ok state with the specified response.
		/// </summary>
		/// <param name="response">The response from Jira.</param>
		public JiraResponse(string response)
		{
			ResponseType = ResponseType.Ok;
			Response = response;
		}

		/// <summary>
		/// Creates a new JiraResponse with the specified non-ok ResponseType and empty Response.
		/// Throws an ArgumentException if ResponseType is Ok.
		/// </summary>
		/// <param name="responseType">The ResponseType, not ResponseType.Ok</param>
		public static implicit operator JiraResponse(ResponseType responseType)
		{
			if (responseType == ResponseType.Ok)
			{
				throw new ArgumentException("Cannot create a JiraResponse with ResponseType.Ok and empty Response");
			}

			return new JiraResponse(responseType);
		}

		/// <summary>
		/// Creates a new JiraResponse with the specified response.
		/// </summary>
		/// <param name="response">The response content</param>
		public static implicit operator JiraResponse(string response)
			=> new(response);

		/// <summary>
		/// Gets the ResponseType of this JiraResponse.
		/// </summary>
		public ResponseType ResponseType { get; }
		
		/// <summary>
		/// Gets the actual Response, only available if ResponseType is Ok.
		/// </summary>
		public string Response { get; }

		/// <summary>
		/// Is the ResponseType in the Ok state?
		/// </summary>
		/// <returns>True if in the Ok state, otherwise false.</returns>
		public bool IsOk()
			=> ResponseType == ResponseType.Ok;
	}
}