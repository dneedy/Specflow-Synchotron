using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Dneedy.Specflow.Synchotron.Testing
{
    public abstract class TestHooks
    {
        [SetUp]
        public void Setup()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var runOnItsOwn = testName.Contains("Sequential");

            if(Synchotron.ResourceIsBlocked(testName, runOnItsOwn))
            {
                while(Synchotron.ResourceIsBlocked(testName))
                {
                    // Do nothing
                }
            }
            // Console.WriteLine($"{(runOnItsOwn ? "S" : "P")} {testName}                     this synchronises, don't use normally");
            
            var duration = runOnItsOwn ? 300 : 100;
            var hz = runOnItsOwn ? 500 : 200;
            Console.Beep(hz, duration);
        }

        /// <summary>
        /// Simlate a test running for an ammount of ticks, so can guarantee parallel run control for tests
        /// </summary>
        /// <param name="tick">Number of ticks to delay the test</param>
        /// <returns></returns>
        protected async Task Pause(int tick)
        {
            // TODO: Verify that this is not too clever and does not stop before returning (eg optimised away)
            var milliseconds = tick * 2000;
            await Task.Delay(milliseconds) ;
        }

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var runOnItsOwn = testName.Contains("Sequential");

            // Console.WriteLine($"- {testName}                     this synchronises, don't use normally");
            
            var duration = runOnItsOwn ? 300 : 100;
            var hz = runOnItsOwn ? 550 : 250;
            Console.Beep(hz, duration);

            Synchotron.ResourceHasFinished(testName);
        }
    }
}