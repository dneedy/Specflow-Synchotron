using NUnit.Framework;
using System.Threading.Tasks;

namespace Dneedy.Specflow.Synchotron.Testing
{
    public class Thread1
        : TestHooks
    {
        [Test, Order(1)]
        public async Task Thread1RunOnTick1()
        {
            await Pause(1);
        }

        [Test, Order(2)]
        public async Task Thread1RunOnTick4Sequential()
        {
            await Pause(1);
        }

        [Test, Order(3)]
        public async Task Thread1RunOnTick6()
        {
            await Pause(1);
        }
    }
}