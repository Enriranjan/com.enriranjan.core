using EnriRanjan.Core;
using EnriRanjan.Core.Unity;
using NUnit.Framework;
using UnityEngine;

namespace EnriRanjan.Core.Editor.Tests
{
    /// <summary>
    /// Verifies that a concrete <see cref="ApplicationBootstrap"/> registers
    /// its bindings into the locator on Awake, and marks tickables as such.
    /// </summary>
    public class ApplicationBootstrapTests
    {
        private interface IFakeService
        {
        }

        private sealed class FakeService : IFakeService
        {
        }

        private sealed class FakeTickable : ITickable
        {
            public int TickCount { get; private set; }

            public void Tick(float deltaTime) => TickCount++;
        }

        private sealed class TestBootstrap : ApplicationBootstrap
        {
            protected override void InstallBindings()
            {
                Register<IFakeService>(new FakeService());
                Register(new FakeTickable());
            }
        }

        private GameObject _gameObject;

        [TearDown]
        public void TearDown()
        {
            if (_gameObject != null)
            {
                Object.DestroyImmediate(_gameObject);
            }
        }

        [Test]
        public void Awake_RegistersBindingsAndMarksApplicationReady()
        {
            _gameObject = new GameObject(nameof(TestBootstrap));
            _gameObject.AddComponent<TestBootstrap>();

            Assert.IsTrue(ApplicationBootstrap.IsReady);
            Assert.IsTrue(ApplicationBootstrap.Locator.Contains<IFakeService>());
        }
    }
}
