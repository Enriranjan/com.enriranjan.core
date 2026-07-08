using UnityEngine;

namespace EnriRanjan.Core.Unity
{
    /// <summary>
    /// Scene composition root: one per gameplay scene. Reads the locator
    /// populated by <see cref="ApplicationBootstrap"/> - it is the only type
    /// allowed to read it - and creates the scene's controllers, injecting
    /// the services they need. Its <c>Awake</c> can assume every system,
    /// service and provider is already initialized, because
    /// <see cref="ApplicationBootstrap"/> only loads this scene after
    /// <see cref="ApplicationBootstrap.InstallBindings"/> has finished. This
    /// base class knows nothing about concrete views or controllers.
    /// </summary>
    public abstract class SceneContextInstaller : MonoBehaviour
    {
        private void Awake()
        {
            if (!ApplicationBootstrap.IsReady)
            {
#if UNITY_EDITOR
                Debug.LogWarning(
                    $"{nameof(SceneContextInstaller)} on '{gameObject.name}' was loaded without " +
                    $"{nameof(ApplicationBootstrap)} having run. This scene must be reached by pressing " +
                    "Play from the Bootstrap scene, not by opening it directly. Disabling this GameObject.",
                    this);
                gameObject.SetActive(false);
                return;
#else
                throw new System.InvalidOperationException(
                    $"{nameof(SceneContextInstaller)} on '{gameObject.name}' requires the application to " +
                    $"have started from the Bootstrap scene: {nameof(ApplicationBootstrap)} has not run.");
#endif
            }

            InstallScene(ApplicationBootstrap.Locator);
        }

        /// <summary>
        /// Creates this scene's controllers and injects the services they
        /// need, reading them from <paramref name="locator"/>.
        /// </summary>
        protected abstract void InstallScene(IReferenceLocator locator);
    }
}
