using System;
using System.Collections.Generic;

namespace EnriRanjan.Core
{
    /// <summary>
    /// Type-keyed registry of systems, services and providers. Reading is
    /// public through <see cref="IReferenceLocator"/>; writing
    /// (<see cref="Register{T}"/>, <see cref="Clear"/>) is internal, so only
    /// the composition root (ApplicationBootstrap, in EnriRanjan.Core.Unity)
    /// and this package's tests can populate it. This class holds no
    /// static/singleton state - the bootstrap owns and holds the instance.
    /// </summary>
    public sealed class ReferenceLocator : IReferenceLocator
    {
        private readonly Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        /// <inheritdoc />
        public T Get<T>()
        {
            if (TryGet(out T instance))
            {
                return instance;
            }

            throw new InvalidOperationException(
                $"No instance of type '{typeof(T)}' is registered in the {nameof(ReferenceLocator)}.");
        }

        /// <inheritdoc />
        public bool TryGet<T>(out T instance)
        {
            if (_instances.TryGetValue(typeof(T), out object value))
            {
                instance = (T)value;
                return true;
            }

            instance = default;
            return false;
        }

        /// <inheritdoc />
        public bool Contains<T>()
        {
            return _instances.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Registers <paramref name="instance"/> under type <typeparamref name="T"/>.
        /// Internal: only the application bootstrap (and tests) may write to
        /// the locator - the compiler enforces this via InternalsVisibleTo.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when an instance of <typeparamref name="T"/> is already registered.
        /// </exception>
        internal void Register<T>(T instance)
        {
            if (_instances.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException(
                    $"An instance of type '{typeof(T)}' is already registered in the {nameof(ReferenceLocator)}.");
            }

            _instances.Add(typeof(T), instance);
        }

        /// <summary>
        /// Removes every registered instance. Internal: intended for tests
        /// and bootstrap teardown.
        /// </summary>
        internal void Clear()
        {
            _instances.Clear();
        }
    }
}
