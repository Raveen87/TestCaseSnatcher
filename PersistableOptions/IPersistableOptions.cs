using Microsoft.Extensions.Options;
using System;

namespace Rajven.PersistableOptions
{
	public interface IPersistableOptions<out T> : IOptions<T>
		where T : class, new()
	{
		/// <summary>
		/// Persists changes to the T instance.
		/// </summary>
		/// <param name="applyChanges">Changes to apply to the T instance, will be persisted.</param>
		void Persist(Action<T> applyChanges);
	}
}