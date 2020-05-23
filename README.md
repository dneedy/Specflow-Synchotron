# Specflow-Synchotron
Within a parallel run, ensure that sequential tests run on their own

        If a parallel test starts, it waits until there are no serial tests before continuing
		When a serial test starts, it waits until no other tests are running, then continues
		The tests are synchronised through a static locking mechanism
		Position them in your test framework as shown below.
		
		public void InTestFrameworksMethodRunBeforeScenario()
        {
            // Run before other code

            var hasSequentialTrait = true; // From testing framework
            var testName = "theTestName"; // From testing framework

            if(Synchotron.NewTestIsBlocked(testName, runOnItsOwn: hasSequentialTrait))
            {
                while(Synchotron.TestIsBlocked(testName))
                {
                    // Do nothing, ideally use a timer to wait, but normally not possible, so just sacrifice a little cpu
                }
            }
        }

        public void InTestFrameworksMethodRunAfterScenario()
        {
            // Run after other code

            var testName = "theTestName";

            Synchotron.TestHasFinished(testName);
        }
