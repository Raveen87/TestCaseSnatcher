using System;
using Rajven.ComExt.EnumeratorExtensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Rajven.ComExt.Tests.Enumerator
{
	public class EnumeratorExtensionsTests
	{
		[Fact]
		public void GetRemaining_AllThreeRemaining_ReturnsAllThreeElements()
		{
			Assert.Equal(3, CreateElements(3).GetEnumerator().GetRemaining().Count());
		}

		[Fact]
		public void GetRemaining_FirstItemOfFiveConsumed_ReturnsRemainingFourElements()
		{
			var enumerator = CreateElements(5).GetEnumerator();
			enumerator.MoveNext();

			Assert.Equal(4, enumerator.GetRemaining().Count());
		}

		[Fact]
		public void GetRemaining_ArgumentIsNull_ArgumentExceptionIsThrown()
		{
			var nullEnumerator = (IEnumerator<string>) null;

			Assert.Throws<ArgumentException>(() => nullEnumerator.GetRemaining().Any());
		}

		[Fact]
		public void GetRemaining_AllElementsAlreadyConsumed_EmptyIEnumerableIsReturned()
		{
			var enumerator = CreateElements(3).GetEnumerator();
			enumerator.MoveNext();
			enumerator.MoveNext();
			enumerator.MoveNext();

			Assert.False(enumerator.GetRemaining().Any());
		}

		[Fact]
		public void GetRemaining_EmtpyEnumerable_NoElementsReturned()
		{
			var emptyEnumerable = Enumerable.Empty<string>();

			Assert.False(emptyEnumerable.GetEnumerator().GetRemaining().Any());
		}

		private IEnumerable<string> CreateElements(int numberOfItems)
		{
			for (int i = 0; i < numberOfItems; i++)
			{
				yield return $"item_{i}";
			}
		}
	}
}