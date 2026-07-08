namespace EnriRanjan.Core.Samples.BasicUsage
{
    /// <summary>
    /// Minimal pure C# system used by the BasicUsage sample. Stands in for a
    /// real gameplay system: no Unity dependency, created once and owned by
    /// the bootstrap for the whole application lifetime.
    /// </summary>
    public sealed class FakeScoreSystem
    {
        private int _score;

        public int CurrentScore => _score;

        public void AddPoints(int points)
        {
            _score += points;
        }
    }
}
