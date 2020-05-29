namespace Dneedy.Specflow.Synchotron
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
 
    public class Synchotron
    {
        private static object SingletonKey = new object();

        /// <summary>
        /// Optional Logging that can be set to track the synchronisation occurring
        /// </summary>
        public static ISynchotronLog GlobalLog { get;  set; }

        /// <summary>
        /// Start tracking a resource
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="runOnItsOwn"></param>
        /// <returns>If the resource is blocked</returns>
        public static bool ResourceIsBlocked(string resourceName, bool runOnItsOwn)
        {
            lock (SingletonKey)
            {
                var resource = new Resource(
                        id: resourceName,
                        runOnItsOwn: runOnItsOwn);

                // Index
                Slots.Add(resource);

                var blocked = Slots.Blocked(resource);

                Log(">", resource);

                return blocked;
            }
        }

        /// <summary>
        /// Check if the resource is still blocked
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static bool ResourceIsBlocked(string resourceName)
        {
            lock (SingletonKey)
            {
                var resource = Slots.GetResource(resourceName);
                if(resource == null)
                {
                    throw new IndexOutOfRangeException($"A resource named {resourceName} has not been setup with a runOnItsOwn context");
                }

                var blocked = Slots.Blocked(resource);
                if (!blocked)
                {
                    Log("*", resource);
                }
                else
                {
                    // Log("?", test); // Only for debugging to prove that block checks are ongoing
                }
                return blocked;
            }
        }

        /// <summary>
        /// Finish tracking a resource
        /// </summary>
        /// <param name="resourceName"></param>
        public static void ResourceHasFinished(string resourceName)
        {
            lock (SingletonKey)
            {
                var resource = Slots.GetResource(resourceName);
                Log("<", resource, finished: true);
                Slots.IHaveFinished(resource);
            }
        }

        private static void Log(string prefix, Resource test, bool finished = false)
        {
            if(GlobalLog == null)
            {
                return;
            }

            var line = $"{DateTimeOffset.Now} {test.ToString(prefix)}";
            GlobalLog.Debug(line);
        }
    }
}
