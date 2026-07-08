using System;

namespace EnriRanjan.Core
{
    /// <summary>
    /// Read-only access to the systems, services and providers registered by
    /// the application bootstrap. This is the only surface a scene context
    /// installer should use to read from the composition root; nothing in
    /// this interface allows writing to it.
    /// </summary>
    public interface IReferenceLocator
    {
        /// <summary>
        /// Returns the registered instance of <typeparamref name="T"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when no instance of <typeparamref name="T"/> has been registered.
        /// </exception>
        T Get<T>();

        /// <summary>
        /// Attempts to retrieve the registered instance of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="instance">
        /// The registered instance, or the default value of <typeparamref name="T"/>
        /// if none was found.
        /// </param>
        /// <returns>True if an instance was found; otherwise false.</returns>
        bool TryGet<T>(out T instance);

        /// <summary>
        /// Returns true if an instance of <typeparamref name="T"/> is registered.
        /// </summary>
        bool Contains<T>();
    }
}
