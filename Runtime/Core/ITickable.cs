namespace EnriRanjan.Core
{
    /// <summary>
    /// Implemented by pure C# objects that need a per-frame update.
    /// The Unity time source only enters the lower layers through
    /// EnriRanjan.Core.Unity.ApplicationBootstrap, which calls <see cref="Tick"/>
    /// on every registered instance from its own Update loop.
    /// </summary>
    public interface ITickable
    {
        /// <summary>
        /// Advances this instance by <paramref name="deltaTime"/> seconds.
        /// </summary>
        /// <param name="deltaTime">Elapsed time since the previous tick, in seconds.</param>
        void Tick(float deltaTime);
    }
}
