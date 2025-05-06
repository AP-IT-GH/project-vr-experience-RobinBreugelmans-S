using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreScript : MonoBehaviour
    {
        static int scoreML = 0;
        static int scorePlayer = 0;

        public static void AddScorePlayer(int scoreToAdd)
        {
            scorePlayer += scoreToAdd;
        }

        public static void AddScoreML(int scoreToAdd)
        {
            scoreML += scoreToAdd;
        }

        public static void ResetScores()
        {
            scoreML = 0;
            scorePlayer = 0;
        }
    }
}