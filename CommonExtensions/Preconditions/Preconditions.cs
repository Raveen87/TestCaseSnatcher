using System;

namespace Rajven.ComExt.ObjectExtensions
{
	public static class Preconditions
	{
		/// <summary>
		/// Verifies that the given argument is not null. Used to verify that a method argument is not null.
		/// </summary>
		/// <typeparam name="T">The type of the argument to check.</typeparam>
		/// <param name="argument">The argument to check.</param>
		/// <param name="argumentName">The name of the argument for the ArgumentException if argument was null.</param>
		/// <exception cref="ArgumentException">Thrown if argument is null.</exception>
		public static void VerifyNotNull<T>(this T argument, string argumentName)
		{
			if (argument is null)
			{
				throw new ArgumentException(argumentName);
			}
		}
	}
}