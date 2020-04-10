using CommonExtensions.IEnumerable;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CommonExtensions.Tests.IEnumerable
{
	public class IEnumerableExtensionsTess
	{
		[Fact]
		public void Duplicates_TwoElementsThatAreDuplicates_ReturnsTheDuplicatedElement()
		{
			var elem = "duplicate";
			var duplicateList = new List<string>() { elem, elem };

			var duplicate = duplicateList.Duplicates();

			Assert.Equal(new List<string>() { elem }, duplicate);
		}

		[Fact]
		public void Duplicates_ThreeElementsThatAreDuplicates_ReturnsTheDuplicatedElement()
		{
			var elem = "duplicate";
			var duplicateList = new List<string>() { elem, elem, elem };

			var duplicate = duplicateList.Duplicates();

			Assert.Equal(new List<string>() { elem }, duplicate);
		}

		[Fact]
		public void Duplicates_ManyElementsWithTwoDuplicates_ReturnsTheTwoDuplicatedElements()
		{
			var duplicateList = new List<int>() { 5, 12, 3, 27, 18, 3, 5 };
			var duplicate = duplicateList.Duplicates();

			Assert.Equal(new List<int>() { 5, 3 }, duplicate);
		}

		[Fact]
		public void Duplicates_ManyElementsWithNoDuplicates_ReturnsEmpty()
		{
			var duplicateList = new List<int>() { 166, 27, 18, 573, 123, 79 };
			var duplicate = duplicateList.Duplicates();

			Assert.Equal(Enumerable.Empty<int>(), duplicate);
		}

		[Fact]
		public void Duplicates_NoElements_ReturnsEmpty()
		{
			var noDuplicates = new List<int>();
			var duplicate = noDuplicates.Duplicates();

			Assert.Equal(Enumerable.Empty<int>(), duplicate);
		}
	}
}