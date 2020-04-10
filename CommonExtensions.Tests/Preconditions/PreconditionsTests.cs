using System;
using Rajven.ComExt.ObjectExtensions;
using Xunit;

namespace Rajven.Common.Tests.ObjectExtensions
{
	public class PreconditionsTests
	{
		[Fact]
		public void ThrowIfNull_NullVariable_ArgumentExceptionIsThrown()
		{
			Assert.Throws<ArgumentException>(() => ((string) null).VerifyNotNull("nullStr"));
		}

		[Fact]
		public void ThrowIfNull_InstanceVariable_NoExceptionThrown()
		{
			"instance".VerifyNotNull("instance");
		}
	}
}