using Rajven.ComExt.ObjectExtensions;
using System.Collections.Generic;

namespace Rajven.ComExt.EnumeratorExtensions
{
	public static class EnumeratorExtensions
	{
		/// <summary>
		/// Returns the remaining elements from this IEnumerator.
		/// </summary>
		/// <typeparam name="T">The type of objects the enumerator enumerates.</typeparam>
		/// <param name="enumerator">The enumerator to get the remaining elements for.</param>
		/// <returns>All the elements that remain in the enumerator.</returns>
		/// <exception cref="System.ArgumentException">Thrown when enumerator is null.</exception>
		public static IEnumerable<T> GetRemaining<T>(this IEnumerator<T> enumerator)
		{
			enumerator.VerifyNotNull("enumerator");

			while (enumerator.MoveNext())
			{
				if (enumerator.Current is not null)
				{
					yield return enumerator.Current;
				}
			}
		}
	}
}