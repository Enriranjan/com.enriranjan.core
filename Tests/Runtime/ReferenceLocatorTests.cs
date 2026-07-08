using System;
using NUnit.Framework;

namespace EnriRanjan.Core.Tests
{
    /// <summary>
    /// Plain NUnit tests for <see cref="ReferenceLocator"/>. Relies on
    /// InternalsVisibleTo (declared in Runtime/Core/AssemblyInfo.cs) to call
    /// the internal Register/Clear members directly.
    /// </summary>
    public class ReferenceLocatorTests
    {
        private interface IFakeService
        {
        }

        private sealed class FakeService : IFakeService
        {
        }

        [Test]
        public void Get_ReturnsRegisteredInstance()
        {
            var locator = new ReferenceLocator();
            var service = new FakeService();
            locator.Register<IFakeService>(service);

            Assert.AreSame(service, locator.Get<IFakeService>());
        }

        [Test]
        public void Get_ThrowsWhenTypeNotRegistered()
        {
            var locator = new ReferenceLocator();

            var exception = Assert.Throws<InvalidOperationException>(() => locator.Get<IFakeService>());
            StringAssert.Contains(nameof(IFakeService), exception.Message);
        }

        [Test]
        public void TryGet_ReturnsFalseWhenNotRegistered()
        {
            var locator = new ReferenceLocator();

            bool found = locator.TryGet(out IFakeService instance);

            Assert.IsFalse(found);
            Assert.IsNull(instance);
        }

        [Test]
        public void TryGet_ReturnsTrueAndInstanceWhenRegistered()
        {
            var locator = new ReferenceLocator();
            var service = new FakeService();
            locator.Register<IFakeService>(service);

            bool found = locator.TryGet(out IFakeService instance);

            Assert.IsTrue(found);
            Assert.AreSame(service, instance);
        }

        [Test]
        public void Contains_ReflectsRegistrationState()
        {
            var locator = new ReferenceLocator();

            Assert.IsFalse(locator.Contains<IFakeService>());

            locator.Register<IFakeService>(new FakeService());

            Assert.IsTrue(locator.Contains<IFakeService>());
        }

        [Test]
        public void Register_ThrowsWhenTypeAlreadyRegistered()
        {
            var locator = new ReferenceLocator();
            locator.Register<IFakeService>(new FakeService());

            Assert.Throws<InvalidOperationException>(() => locator.Register<IFakeService>(new FakeService()));
        }

        [Test]
        public void Clear_RemovesAllRegisteredInstances()
        {
            var locator = new ReferenceLocator();
            locator.Register<IFakeService>(new FakeService());

            locator.Clear();

            Assert.IsFalse(locator.Contains<IFakeService>());
        }
    }
}
