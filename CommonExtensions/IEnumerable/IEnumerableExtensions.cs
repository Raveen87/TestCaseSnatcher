using System.Collections.Generic;
using System.Linq;

namespace CommonExtensions.IEnumerable
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Finds duplicated elements and returns a new IEnumerable that contains each duplicated element once.
		/// </summary>
		/// <typeparam name="T">The type of objects to find duplicates of.</typeparam>
		/// <param name="enumerable">This enumerable to find duplicates in.</param>
		/// <returns>A new IEnumerable with each duplicated element once.</returns>
		public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> enumerable)
			=> enumerable.GroupBy(elem => elem)
				.Where(grp => grp.Count() > 1)
				.Select(duplicate => duplicate.Key);
	}
}