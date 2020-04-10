using Flurl.Http;
using Serilog;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TestCaseSnatcher.Jira.Json;
using TestCaseSnatcher.Jira.Models;

namespace TestCaseSnatcher.Jira.Services
{
	/// <summary>
	/// Handles HTTP communication with Jira.
	/// </summary>
	internal class Querier : IQuerier
	{
		private readonly ILogger _logger;
		private readonly string _username;
		private readonly string _password;

		public Querier(string username, string password)
		{
			_logger = Log.Logger.ForContext<Querier>();
			_username = username;
			_password = password;

			FlurlHttp.Configure(settings =>
			{
				var serializerOptions = new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				};
				settings.JsonSerializer = new DefaultSerializer(serializerOptions);
			});
		}

		/// <summary>
		/// Gets a Jira resource.
		/// </summary>
		/// <param name="address">The address of the resource to get.</param>
		/// <returns>A JiraResponse with the returned content or error.</returns>
		public async Task<JiraResponse> GetAsync(string address)
		{
			_logger.Debug("Getting: {QueryAddress}", address);

			try
			{
				return await address
					.WithBasicAuth(_username, _password)
					.GetStringAsync()
					.ConfigureAwait(false);
			}
			catch (FlurlHttpException e)
			{
				return MapStatusCodeToResponseType(e.Call.HttpResponseMessage.StatusCode);
			}
		}

		/// <summary>
		/// Puts a Jira resource.
		/// </summary>
		/// <param name="address">The address of the resource to get.</param>
		/// <param name="body">The object that will be serialized as body to put.</param>
		/// <returns>A JiraResponse with the returned content or error.</returns>
		public async Task<JiraResponse> PutAsync(string address, object body)
		{
			_logger.Debug("Putting: {QueryAddress}", address);

			try
			{
				return await address
					.WithBasicAuth(_username, _password)
					.PutJsonAsync(body)
					.ReceiveString()
					.ConfigureAwait(false);
			}
			catch (FlurlHttpException e)
			{
				return MapStatusCodeToResponseType(e.Call.HttpResponseMessage.StatusCode);
			}
		}

		private ResponseType MapStatusCodeToResponseType(HttpStatusCode? responseCode)
		{
			var responseType = responseCode switch
			{
				null => ResponseType.TimedOut,
				HttpStatusCode.NotFound => ResponseType.NotFound,
				HttpStatusCode.Unauthorized => ResponseType.NotAuthorized,
				_ => ResponseType.OtherNonOk
			};

			if (responseType == ResponseType.OtherNonOk)
			{
				_logger.Warning("Received other non-Ok status code: {StatusCode}", responseCode);
			}

			return responseType;
		}
	}
}