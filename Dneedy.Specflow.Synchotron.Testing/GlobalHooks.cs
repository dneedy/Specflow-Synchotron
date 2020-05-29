using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dneedy.Specflow.Synchotron.Testing
{
    [SetUpFixture]
    public class GlobalHooks
    {
        private static Logger _logger = new Logger();

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("GLOBAL SETUP");

            Synchotron.GlobalLog = _logger;  
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Console.Write("GLOBAL TEARDOWN");
        }
    }
}
