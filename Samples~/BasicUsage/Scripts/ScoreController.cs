namespace EnriRanjan.Core.Samples.BasicUsage
{
    /// <summary>
    /// Controller for the BasicUsage sample. Implements
    /// <see cref="ScoreView.IListener"/> to react to view input and pushes
    /// updated state back to the view.
    /// </summary>
    public sealed class ScoreController : ScoreView.IListener
    {
        private readonly ScoreView _view;
        private readonly ScoreService _scoreService;

        public ScoreController(ScoreView view, ScoreService scoreService)
        {
            _view = view;
            _scoreService = scoreService;

            _view.SetListener(this);
            _view.DisplayScore(_scoreService.CurrentScore);
        }

        public void OnAddPointsRequested(int points)
        {
            _scoreService.RegisterPoints(points);
            _view.DisplayScore(_scoreService.CurrentScore);
        }
    }
}
