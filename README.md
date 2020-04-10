# TestCaseSnatcher

Automates taking test cases for the Jira plugin [Zephyr Scale](https://marketplace.atlassian.com/apps/1213259/zephyr-scale-test-management-for-jira) (previously known as TM4J Test Management for Jira).

Implemented as a Worker Service, so install as a service or run in a terminal. Will periodically check if a new test run has been published, and snatch you those precious test cases you want the most.

## Usage

1) Configure the required settings
2) Install the application as a service, or run it in a terminal if you prefer
3) Wait for the next test run to be published

Will only snatch test cases from test runs that are not in the state "Done".


## Configuration

The configuration is available in `appsettings.json`.
There are a few things you need to configure to get it running. Namely the address, authentication details, id of the next test run and test cases.

So if your previous test run address was:
https://jira.mycompany.com/secure/Tests.jspa#/testCycle/TCM-C285

And your log in is `johndoe` / `monkey`.

You can then configure the following values:

"BaseUrl": "https://jira.mycompany.com"  
"User": "johndoe"  
"Password": "monkey"  
"TestRunPrefix": "TCM-C"  
"ActiveTestRunId": 285

If you start the application it will try to access the test run TCM-C285. If that test run is already done, it will sleep for a configurable amount of time, and then try to access TCM-C286. If the test run does not yet exist, it will sleep and try that same one again until it gets a valid test run back.
When it finds a test run that is not yet in state `Done`, it will optionally verify that the description for the test run contains the value specified in `DescriptionFilter`. This is useful if you have test runs for different products published under the same schema, and you are only interested in some of them.

It will then try to snatch _unassigned_ test cases, up to maximum number of test cases as specified by `NumberOfTestCasesToTake`. It will first take the top-most test case specified in `PrioritizedTestCases`, if unassigned.

Example with explanations:

```
{
  "AppSettings": {
    "JiraSettings": {
      "BaseUrl": "https://your.jira.host",      // Your Jira server
      "User": "yourusername",                   // Your Jira username
      "Password": "yourpassword"                // Your Jira password
    },
    "TestSettings": {
      "DescriptionFilter": "must-contain-this", // Optional, default empty: The test run description must contain this string in order to be considered relevant
      "MinimumAgeMinutes": 60,                  // Optional, default 0: If you don't want to make it too obvious you're snatching, will wait to snatch if the test run was created less than this time ago
      "TestRunPrefix": "TCM-C",                 // The prefix for your test runs, i.e. your test run project name, used to build the URLs
      "ActiveTestRunId": 273                    // The active test run to try and snatch from, will automatically be updated after finished with one round
    },
    "DryRun": false,                            // Optional, default false: Enable to don't snatch test cases, logging will be exactly like if it tried to snatch, all test cases simulated successfully snatched
    "SleepMinutes": 1,                          // Optional, default 60: Number of minutes to sleep between checking if a new test run has been created
    "NumberOfTestCasesToTake": 9,               // Maximum number of test cases to take
    "PrioritizedTestCases": [                   // An ordered list of prioritized test cases. Will only every try to snatch test cases in this list, from top to bottom of unassigned test cases
      "TESTCASEID-1",
      "TESTCASEID-5",
      "TESTCASEID-12",
    ]
  }
}
```

## Technologies

.NET 5