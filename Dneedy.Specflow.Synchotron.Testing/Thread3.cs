using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dneedy.Specflow.Synchotron.Testing
{
    public class Thread3 : TestHooks
    {
        [Test, Order(1)]
        public async Task Thread3RunTick1()
        {
            await Pause(2);
        }

        [Test, Order(2)]
        public async Task Thread3RunOnTick5Sequential()
        {
            await Pause(1);
        }

        [Test, Order(3)]
        public async Task Thread3RunOnTick6()
        {
            await Pause(1);
        }

        [Test, Order(4)]
        public void Thread3RunOnTick7EndOfRun()
        {
            // Output log for debugging purposes
            Logger logger = (Logger)Synchotron.GlobalLog;
            logger.Lines.ForEach(l => 
            { 
                TestContext.WriteLine(l); 
            });

            // Arrange
            var p1a = Position("P r] Thread3RunTick1");
            var p1b = Position("P r] Thread2RunOnTick1");
            var p1c = Position("P r] Thread1RunOnTick1");
                     
            var s2 = Position("S r] Thread1RunOnTick4Sequential");
                     
            var s3 = Position("S r] Thread3RunOnTick5Sequential");

            var p4a = Position("P r] Thread2RunOnTick6");
            var p4b = Position("P r] Thread1RunOnTick6");
            var p4c = Position("P r] Thread3RunOnTick6");

            TestContext.WriteLine("Line Scanning .....");
            TestContext.WriteLine($"    p1({p1a},{p1b},{p1c}) s2({s2}) s3({s3}) p4({p4a},{p4b},{p4c})");

            // ASSERT

            /// p1* may run in any order before s2
            Assert.Greater(s2, p1a);
            Assert.Greater(s2, p1b);
            Assert.Greater(s2, p1c);

            // s2 must run before s3
            Assert.Greater(s3, s2);

            // s3 must run before all p4*
            Assert.Greater(p4a, s3);
            Assert.Greater(p4b, s3);
            Assert.Greater(p4c, s3);
        }

        private int Position(string find)
        {
            Logger logger = (Logger)Synchotron.GlobalLog;
            var pos = 0;
            foreach(var line in logger.Lines)
            {
                if(line.Contains(find))
                {
                    break;
                }
                pos++;
            }
            if (pos > logger.Lines.Count)
            {
                throw new IndexOutOfRangeException($"Could not find resource at stage {find}");
            }

            return pos;
        }
    }
}