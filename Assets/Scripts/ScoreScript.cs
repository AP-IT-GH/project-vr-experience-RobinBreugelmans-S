using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreScript : MonoBehaviour
    {
        int scoreML = 0;
        int scorePlayer = 0;

        [SerializeField]
        ScoreBoard scoreBoardML;
        [SerializeField]
        ScoreBoard scoreBoardPlayer;

        public void AddScorePlayer(int scoreToAdd)
        {
            scorePlayer += scoreToAdd;
            scoreBoardPlayer.AddScore(scoreToAdd);
        }

        public void AddScoreML(int scoreToAdd)
        {
            scoreML += scoreToAdd;
            scoreBoardML.AddScore(scoreToAdd);
        }

        public void ResetScores()
        {
            scoreML = 0;
            scorePlayer = 0;
            scoreBoardML.ResetScore();
            scoreBoardPlayer.ResetScore();
        }
    }
}