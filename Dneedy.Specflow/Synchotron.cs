namespace Dneedy.Specflow.Synchotron
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
 
    public class Synchotron
    {
        private static object GlobalKey = new object();

        /// <summary>
        /// Must be called the first time
        /// </summary>
        /// <param name="testName"></param>
        /// <param name="runOnItsOwn"></param>
        /// <returns></returns>
        public static bool NewTestIsBlocked(string testName, bool runOnItsOwn)
        {
            lock (GlobalKey)
            {
                var test = new Synch(
                        id: testName,
                        runOnItsOwn: runOnItsOwn);
                var blocked = Synch.Blocked(test);
                Log("+", test);
                return blocked;
            }
        }

        /// <summary>
        /// When Added and could not carry on, this is subsequenly polled
        /// </summary>
        /// <param name="testName"></param>
        /// <returns></returns>
        public static bool TestIsBlocked(string testName)
        {
            lock (GlobalKey)
            {
                var test = Synch.GetLock(testName);
                var blocked = Synch.Blocked(test);
                if (!blocked)
                {
                    Log(" ", test);
                }
                else
                {
                    // Log("?", test); // Only for debugging to prove that block checks are ongoing
                }
                return blocked;
            }
        }

        /// <summary>
        /// When test has finished
        /// </summary>
        /// <param name="testName"></param>
        public static void TestHasFinished(string testName)
        {
            lock (GlobalKey)
            {
                var test = Synch.GetLock(testName);
                Log("-", test, finished: true);
                Synch.IHaveFinished(test);
            }
        }

        private static void Log(string prefix, Synch test, bool finished = false)
        {
            var detail = finished ? test.ToStringForFinished() : test.ToString();
            var line = $"{DateTimeOffset.Now} {prefix} {detail}";
            System.IO.File.AppendAllText(@"c:\logs\AutomationSynchOngoing.txt", $"{line}{Environment.NewLine}");
        }

        /// <summary>
        /// Internal Logic for Index of 
        /// </summary>
        private class Synch
        {
            public enum TestState { Waiting, Running }
            public enum TestType { Sequential, Parallel }

            // Instance
            public string Id { get; private set; }
            public TestType Type { get; private set; }
            public TestState State { get; private set; }
            private int Slot { get; set; }

            // Index
            private static List<Synch> _lockIndex = new List<Synch>();

            // Index Public
            public static Synch GetLock(string testName)
            {
                return _lockIndex.SingleOrDefault(i => i.Id == testName);
            }
            public static int Count { get { return _lockIndex.Count; } }

            public static bool Blocked(Synch test)
            {
                if (test.Type == TestType.Sequential)
                {
                    return ThereIsSomethingRunning(test);
                }
                return ThereAreSequentials(test);
            }
            public static void IHaveFinished(Synch test)
            {
                _lockIndex.Remove(test);
            }

            // Index Private
            private static bool ThereIsSomethingRunning(Synch test)
            {
                if (_lockIndex.Any(i => i.Id != test.Id && i.State == TestState.Running))
                {
                    return true;
                }
                test.State = TestState.Running;
                return false;
            }
            private static bool ThereAreSequentials(Synch test)
            {
                if (_lockIndex.Any(i => i.Id != test.Id && i.Type == TestType.Sequential))
                {
                    return true;
                }
                test.State = TestState.Running;
                return false;
            }
            private static int NextFreeSlot()
            {
                var slot = 0;
                for (slot = 0; true; slot++)
                {
                    if (_lockIndex.All(index => index.Slot != slot))
                    {
                        break;
                    }
                }
                return slot;
            }

            // Constructor
            public Synch(string id, bool runOnItsOwn)
            {
                // Instance
                Id = id;
                Type = runOnItsOwn ? TestType.Sequential : TestType.Parallel;

                State = TestState.Waiting;
                Slot = NextFreeSlot();

                // Index
                _lockIndex.Add(this);
            }

            public override string ToString()
            {
                return $"{ToStringForState()} {ToStringForSlot()}";
            }
            public string ToStringForFinished()
            {
                return $"{ToStringForStateBlank()} {ToStringForSlot()}";
            }
            private string ToStringForState()
            {
                return $"{State.ToString().Substring(0, 1)}";
            }
            private string ToStringForStateBlank()
            {
                return $" ";
            }
            private string ToStringForSlot()
            {
                var column = this.Slot * 8;
                return $"{Type.ToString().Substring(0, 1)} {_lockIndex.Count()} -> {" ".PadLeft(column, ' ')}{Id}";
            }
        }
    }
}
