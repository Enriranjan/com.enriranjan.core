using NUnit.Framework;

namespace EnriRanjan.Core.Tests
{
    /// <summary>
    /// Plain NUnit tests for <see cref="ITickable"/> using a fake implementation.
    /// </summary>
    public class ITickableTests
    {
        private sealed class FakeTickable : ITickable
        {
            public float LastDeltaTime { get; private set; }
            public int TickCount { get; private set; }

            public void Tick(float deltaTime)
            {
                LastDeltaTime = deltaTime;
                TickCount++;
            }
        }

        [Test]
        public void Tick_RecordsLastDeltaTimeAndCount()
        {
            var tickable = new FakeTickable();

            tickable.Tick(0.5f);

            Assert.AreEqual(0.5f, tickable.LastDeltaTime);
            Assert.AreEqual(1, tickable.TickCount);
        }

        [Test]
        public void Tick_CanBeCalledMultipleTimes()
        {
            var tickable = new FakeTickable();

            tickable.Tick(0.1f);
            tickable.Tick(0.2f);

            Assert.AreEqual(0.2f, tickable.LastDeltaTime);
            Assert.AreEqual(2, tickable.TickCount);
        }
    }
}
