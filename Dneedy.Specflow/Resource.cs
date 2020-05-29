using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dneedy.Specflow
{
    /// <summary>
    /// Something that needs synchronising
    /// </summary>
    internal class Resource
    {
        // Instance
        public string Id { get; private set; }
        public ResourceType Type { get; private set; }
        public ResourceState State { get; set; }
        public int Slot { get; private set; }

        public Resource(string id, bool runOnItsOwn)
        {
            // Instance
            Id = id;
            Type = runOnItsOwn ? ResourceType.Sequential : ResourceType.Parallel;

            State = ResourceState.Waiting;
            Slot = Slots.NextFreeSlot();
        }

        public override string ToString()
        {
            return ToString(" ");
        }
        public string ToString(string prefix)
        {
            var type = Type.ToString().Substring(0, 1);
            var state = State.ToString().Substring(0, 1).ToLowerInvariant();
            var line = PadForColumn($"[{prefix} {type} {state}] {Id}");
            return $" Total Slots {Slots.Count} with Slot {Slot} [{prefix} {type} {state}] = {line}";
        }
        private string PadForColumn(string text)
        {
            var column = Slot * 30;
            return " ".PadLeft(column, ' ') + text;
        }
    }
}
