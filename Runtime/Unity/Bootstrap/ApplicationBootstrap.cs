using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnriRanjan.Core.Unity
{
    /// <summary>
    /// Composition root of the application. Lives on a GameObject in a
    /// dedicated, empty Bootstrap scene: on <c>Awake</c> it creates the
    /// <see cref="ReferenceLocator"/>, calls the abstract
    /// <see cref="InstallBindings"/> so a concrete subclass can create and
    /// register every system, service and provider the app needs, and only
    /// once that finishes does it load the initial game scene. This is the
    /// only type allowed to write into the locator.
    /// </summary>
    public abstract class ApplicationBootstrap : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Scene loaded (in Single mode) right after InstallBindings finishes. Leave empty to load nothing, e.g. in tests.")]
        private string initialSceneName;

        private readonly List<ITickable> _tickables = new List<ITickable>();
        private ReferenceLocator _locator;

        /// <summary>
        /// True once <see cref="InstallBindings"/> has finished running and
        /// <see cref="Locator"/> is safe to read from.
        /// </summary>
        public static bool IsReady { get; private set; }

        /// <summary>
        /// Raised right after <see cref="IsReady"/> becomes true, before the
        /// initial scene load is requested.
        /// </summary>
        public static event Action Ready;

        /// <summary>
        /// Read-only locator populated by the bootstrap. Consumed by
        /// <see cref="SceneContextInstaller"/> once <see cref="IsReady"/> is true.
        /// </summary>
        internal static IReferenceLocator Locator { get; private set; }

        // Editor "Enter Play Mode" settings can disable domain reload, in
        // which case these statics would otherwise leak IsReady/Locator from
        // a previous play session into the next one.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticStateOnLoad()
        {
            IsReady = false;
            Locator = null;
            Ready = null;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _locator = new ReferenceLocator();
            Locator = _locator;

            InstallBindings();

            IsReady = true;
            Ready?.Invoke();

            if (!string.IsNullOrEmpty(initialSceneName))
            {
                SceneManager.LoadScene(initialSceneName, LoadSceneMode.Single);
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < _tickables.Count; i++)
            {
                _tickables[i].Tick(deltaTime);
            }
        }

        /// <summary>
        /// Creates and registers every system, service and provider the
        /// application needs. Called once, from <c>Awake</c>, before the
        /// initial scene loads.
        /// </summary>
        protected abstract void InstallBindings();

        /// <summary>
        /// Registers <paramref name="instance"/> under type <typeparamref name="T"/>
        /// in the locator. If <paramref name="instance"/> implements
        /// <see cref="ITickable"/>, it is also added to the per-frame tick list
        /// driven by this bootstrap's Update.
        /// </summary>
        protected void Register<T>(T instance)
        {
            _locator.Register(instance);

            if (instance is ITickable tickable)
            {
                _tickables.Add(tickable);
            }
        }
    }
}
