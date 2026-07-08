using UnityEngine;

namespace EnriRanjan.Core.Samples.BasicUsage
{
    /// <summary>
    /// Passive view for the BasicUsage sample. Has no game logic of its own:
    /// it forwards player input to its listener and displays whatever the
    /// listener tells it to display.
    /// </summary>
    public sealed class ScoreView : MonoBehaviour
    {
        /// <summary>Implemented by the controller driving this view.</summary>
        public interface IListener
        {
            void OnAddPointsRequested(int points);
        }

        [SerializeField]
        private int pointsPerClick = 10;

        private IListener myListener;

        public void SetListener(IListener listener)
        {
            myListener = listener;
        }

        /// <summary>Wire this up to a UI Button's OnClick in the Inspector.</summary>
        public void OnAddPointsButtonPressed()
        {
            myListener?.OnAddPointsRequested(pointsPerClick);
        }

        public void DisplayScore(int score)
        {
            Debug.Log($"[BasicUsage] Score: {score}");
        }
    }
}
