using System.Threading.Tasks;
using TestCaseSnatcher.Jira.Models;

namespace TestCaseSnatcher.Jira.Services
{
	public interface IQuerier
	{
		/// <summary>
		/// Asynchronously modify a Jira resource with a Put request.
		/// </summary>
		/// <param name="address">The address to modify.</param>
		/// <param name="body"></param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		Task<JiraResponse> PutAsync(string address, object body);

		/// <summary>
		/// Asynchronously access a Jira resource with a Get reqeust.
		/// </summary>
		/// <param name="address">The address to query.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		Task<JiraResponse> GetAsync(string address);
	}
}