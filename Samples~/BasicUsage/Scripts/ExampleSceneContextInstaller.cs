using EnriRanjan.Core.Unity;
using UnityEngine;

namespace EnriRanjan.Core.Samples.BasicUsage
{
    /// <summary>
    /// Example scene installer for the BasicUsage sample. Place on a
    /// GameObject in the gameplay scene loaded by
    /// <see cref="ExampleApplicationBootstrap"/>. Reads
    /// <see cref="ScoreService"/> from the locator and wires it into the
    /// scene's controller together with the scene's view.
    /// </summary>
    public sealed class ExampleSceneContextInstaller : SceneContextInstaller
    {
        [SerializeField]
        private ScoreView scoreView;

        protected override void InstallScene(IReferenceLocator locator)
        {
            var scoreService = locator.Get<ScoreService>();
            _ = new ScoreController(scoreView, scoreService);
        }
    }
}
