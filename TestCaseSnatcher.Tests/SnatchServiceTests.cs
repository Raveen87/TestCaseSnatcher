using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestCaseSnatcher.Jira.Services;
using TestCaseSnatcher.Models;
using TestCaseSnatcher.Services;
using TestCaseSnatcher.Tests.Data;
using Xunit;

namespace TestCaseSnatcher.Tests
{
	public class SnatchServiceTests : IDisposable
	{
		private readonly Mock<ITestCaseHandler> _testCaseHandler;

		public SnatchServiceTests()
		{
			_testCaseHandler = new Mock<ITestCaseHandler>();
		}

		[Fact]
		public void AttemptSnatch_NoAssignedTestCases_IsSuccess()
		{
			var unassignedTestRun = TestData.CreateTestRun().WithUnassignedTestCases();

			_testCaseHandler.Setup(tch => tch.GetTestRunAsync(Default.TestRunKey))
				.Returns(Task.FromResult(unassignedTestRun));
			var snatchService = new SnatchService(Default.Settings, _testCaseHandler.Object);

			var status = Task.Run(() => snatchService.AttemptSnatchAsync(unassignedTestRun.Key)).Result;

			Assert.Equal(SnatchStatus.Success, status);
		}

		[Fact]
		public void AttemptSnatch_DescrptionDoesNotContainFilter_SnatchIsNotAttempted()
		{
			var unassignedTestRun = TestData.CreateTestRun().WithUnassignedTestCases();
			var settings = Default.Settings;
			settings.TestSettings.DescriptionFilter = "Error";
			var snatchService = new SnatchService(settings, _testCaseHandler.Object);

			_testCaseHandler.Setup(tch => tch.GetTestRunAsync(Default.TestRunKey))
				.Returns(Task.FromResult(unassignedTestRun));

			var status = Task.Run(() => snatchService.AttemptSnatchAsync(unassignedTestRun.Key)).Result;

			Assert.Equal(SnatchStatus.NotAvailable, status);
		}

		[Fact]
		public void AttemptSnatch_TestRunIsInStatusDone_SnatchIsNotAttempted()
		{
			var unassignedTestRun = TestData.CreateTestRun("Done").WithUnassignedTestCases();
			var snatchService = new SnatchService(Default.Settings, _testCaseHandler.Object);

			_testCaseHandler.Setup(tch => tch.GetTestRunAsync(Default.TestRunKey))
				.Returns(Task.FromResult(unassignedTestRun));

			var status = Task.Run(() => snatchService.AttemptSnatchAsync(unassignedTestRun.Key)).Result;

			Assert.Equal(SnatchStatus.NotAvailable, status);
		}

		[Fact]
		public void AttemptSnatch_TestRunIsNotOldEnough_SnatchIsNotAttempted()
		{
			var now = DateTime.Now;
			var settings = Default.Settings;
			settings.TestSettings.MinimumAgeMinutes = 60;

			var unassignedTestRun = TestData.CreateTestRun().WithCreatedTime(now.AddMinutes(-59));
			var snatchService = new SnatchService(settings, _testCaseHandler.Object);

			_testCaseHandler.Setup(tch => tch.GetTestRunAsync(Default.TestRunKey))
				.Returns(Task.FromResult(unassignedTestRun));

			var status = Task.Run(() => snatchService.AttemptSnatchAsync(unassignedTestRun.Key)).Result;

			Assert.Equal(SnatchStatus.NotAvailable, status);
		}

		[Fact]
		public void AttemptSnatch_TestRunAlreadyHasTestCasesAssignedToSnatchingUser_SnatchIsNotAttempted()
		{
			var unassigned = TestData.CreateTestCases().CreateTestCases(10);
			var assigned = TestData.CreateTestCases().CreateTestCases(1, Default.User);

			var testRun = TestData.CreateTestRun().WithTestCases(unassigned.Concat(assigned));
			var snatchService = new SnatchService(Default.Settings, _testCaseHandler.Object);

			_testCaseHandler.Setup(tch => tch.GetTestRunAsync(Default.TestRunKey))
				.Returns(Task.FromResult(testRun));

			var status = Task.Run(() => snatchService.AttemptSnatchAsync(testRun.Key)).Result;

			Assert.Equal(SnatchStatus.NotAvailable, status);
		}

		public void Dispose()
		{

		}
	}
}