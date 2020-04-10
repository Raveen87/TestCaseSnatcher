using TestCaseSnatcher.Tests.Data;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using TestCaseSnatcher.Services;

namespace TestCaseSnatcher.Tests
{
	public class TestCaseSnatcherTests
	{
		[Fact]
		public void SnatchTestCase_OnlyFiveTestCasesArePrioritized_OnlyThoseFiveSnatched()
		{
			var testCases = TestData.CreateTestCases().CreateTestCases(10).ToList();
			var firstFiveTestCases = testCases.Take(5);

			var report = SnatchHelper.SnatchTestCasesAsync(testCases, firstFiveTestCases.Select(tc => tc.Key), (_) => Task.FromResult(true), 10).Result;

			Assert.Equal(firstFiveTestCases, report.Successful);
		}

		[Fact]
		public void SnatchTestCase_AllTestCasesAvilableAndTakeMaxSetTo5_OnlyFirstFiveSnatched()
		{
			var testCases = TestData.CreateTestCases().CreateTestCases(10).ToList();
			var firstFiveTestCases = testCases.Take(5);

			var report = SnatchHelper.SnatchTestCasesAsync(testCases, testCases.Select(tc => tc.Key), (_) => Task.FromResult(true), 5).Result;

			Assert.Equal(firstFiveTestCases, report.Successful);
			Assert.Equal(testCases.Except(firstFiveTestCases), report.Skipped);
		}

		[Fact]
		public void SnatchTestCase_FiveMostPrioritizedAlreadyTaken_NextFivePrioritizedTaken()
		{
			var assignedTestCases = TestData.CreateTestCases().CreateTestCases(5, "otheruser").ToList();
			var availableTestCases = TestData.CreateTestCases().CreateTestCases(5).ToList();
			var testCases = assignedTestCases.Concat(availableTestCases);

			var report = SnatchHelper.SnatchTestCasesAsync(testCases, testCases.Select(tc => tc.Key), (_) => Task.FromResult(true), 5).Result;

			Assert.Equal(availableTestCases, report.Successful);
		}

		[Fact]
		public void SnatchTestCase_SnatchFailsForOnlyFivePrioritized_ThoseFiveAreInFailedReport()
		{
			var testCases = TestData.CreateTestCases().CreateTestCases(10).ToList();
			var firstFiveTestCases = testCases.Take(5);

			var report = SnatchHelper.SnatchTestCasesAsync(testCases, firstFiveTestCases.Select(tc => tc.Key), (_) => Task.FromResult(false), 10).Result;

			Assert.Equal(firstFiveTestCases, report.Failed);
		}

		[Fact]
		public void SnatchTestCase_AllTestCasesAlreadyTaken_NoneTakenOrFailedOrSkipped()
		{
			var assignedTestCases = TestData.CreateTestCases().CreateTestCases(15, "otheruser").ToList();

			var report = SnatchHelper.SnatchTestCasesAsync(assignedTestCases, assignedTestCases.Select(tc => tc.Key), (_) => Task.FromResult(true), 15).Result;

			Assert.Empty(report.Successful);
			Assert.Empty(report.Failed);
			Assert.Empty(report.Skipped);
		}
	}
}