using NUnit.Framework;
using System.Threading.Tasks;

namespace Dneedy.Specflow.Synchotron.Testing
{
    public class Thread2 : TestHooks
    {
        [Test, Order(1)]
        public async Task Thread2RunOnTick1()
        {
            await Pause(2);
        }

        [Test, Order(2)]
        public async Task Thread2RunOnTick6()
        {
            await Pause(1);
        }
    }
}