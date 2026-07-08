using EnriRanjan.Core.Unity;

namespace EnriRanjan.Core.Samples.BasicUsage
{
    /// <summary>
    /// Example bootstrap for the BasicUsage sample. Place on a GameObject in
    /// an empty "Bootstrap" scene set as the project's entry point. It
    /// creates the sample's system/service and registers the service so the
    /// initial gameplay scene can consume it through
    /// <see cref="ExampleSceneContextInstaller"/>.
    /// </summary>
    public sealed class ExampleApplicationBootstrap : ApplicationBootstrap
    {
        protected override void InstallBindings()
        {
            var scoreSystem = new FakeScoreSystem();
            var scoreService = new ScoreService(scoreSystem);

            Register(scoreService);
        }
    }
}
