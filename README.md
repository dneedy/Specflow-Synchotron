# Specflow-Synchotron

Within a parallel run, ensure that sequential tests run on their own
- If a parallel test starts, it waits until there are no serial tests before continuing
- If a serial test starts, it waits until no other tests are running, then continues
- The tests are synchronised through a static blocking lock mechanism
- Position them in your test framework as shown below.
- You can receive tracing/logging if you implement ISynchotronLog and set Synchotron.GlobalLog = yourImplementation

The readme files in the code and tests have more details

## Exxample Integration with SpecFlow

```
namespace X.Y.Z
{
	[Binding]
	public class BeforeAndAfter : TechTalk.Specflow.Steps
	{
		// Placed as early as possible in the scenario pipeline
		// Unfortunately even here, the test timer has already started, so the wait time is included in the runtime stats given
		// We assume that you will have your own injected repository that returns the ScenarioContext
		[BeforeScenario(Order = 1)]
		public void BeforeAllScenarios()
		{
			var testName = _repository.scenarioContext.ScenarioInfo.Title;
			var runOnItsOwn = _repository.ScenarioContext.ScenarioInfo.Tags.Contains("sequential") || _repository.featureContext.FeatureInfo.Tags.Contains("sequential");
			if (Synchotron.NewTestIsBlocked(testName, runOnItsOwn))
			{
				while (Synchotron.TestIsBlocked(testName))
				{
					// Blocked (Do not put in thread sleeps here as they will stop other tests, raised cpu is the lesser evil I think)
					// Normally would use timers to pause, but this is not async and we are not able to block until the timer returns here (don't think so)
				}
			}
		}

		// Placed as late as possible in the scenario pipeline
		[AfterScenario(Order = 2)]
		private void SynchotronCleanup()
		{
			var testName = _repository.scenarioContext.ScenarioInfo.Title;
			Synchotron.TestHasFinished(testName);
		}
		```
	}
}
