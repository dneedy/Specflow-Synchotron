using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dneedy.Specflow
{
    /// <summary>
    /// Internal Logic for Index of 
    /// </summary>
    internal static class Slots
    {
        // Index
        private static List<Resource> _lockIndex = new List<Resource>();

        public static void Add(Resource resource)
        {
            _lockIndex.Add(resource);
        }

        public static Resource GetResource(string resourceName)
        {
            return _lockIndex.SingleOrDefault(i => i.Id == resourceName);
        }
        public static int Count { get { return _lockIndex.Count; } }

        public static bool Blocked(Resource resource)
        {
            if (resource.Type == ResourceType.Sequential)
            {
                return ThereIsSomethingRunning(resource);
            }
            return ThereAreSequentials(resource);
        }
        public static void IHaveFinished(Resource test)
        {
            _lockIndex.Remove(test);
        }

        public static int NextFreeSlot()
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


        private static bool ThereIsSomethingRunning(Resource test)
        {
            if (_lockIndex.Any(i => i.Id != test.Id && i.State == ResourceState.Running))
            {
                return true;
            }
            test.State = ResourceState.Running;
            return false;
        }
        private static bool ThereAreSequentials(Resource test)
        {
            if (_lockIndex.Any(i => i.Id != test.Id && i.Type == ResourceType.Sequential))
            {
                return true;
            }
            test.State = ResourceState.Running;
            return false;
        }
    }
}
