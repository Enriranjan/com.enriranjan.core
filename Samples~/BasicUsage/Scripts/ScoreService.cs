namespace EnriRanjan.Core.Samples.BasicUsage
{
    /// <summary>
    /// Application-layer service exposing score use cases to controllers.
    /// Owns a <see cref="FakeScoreSystem"/>; created once by the bootstrap
    /// and registered in the locator for the whole application lifetime.
    /// </summary>
    public sealed class ScoreService
    {
        private readonly FakeScoreSystem _scoreSystem;

        public ScoreService(FakeScoreSystem scoreSystem)
        {
            _scoreSystem = scoreSystem;
        }

        public int CurrentScore => _scoreSystem.CurrentScore;

        public void RegisterPoints(int points)
        {
            _scoreSystem.AddPoints(points);
        }
    }
}
