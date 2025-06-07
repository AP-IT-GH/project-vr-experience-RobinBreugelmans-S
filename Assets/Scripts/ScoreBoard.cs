using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    TextMeshPro tmp;
    int score = 0;

    public void AddScore(int score)
    {
        this.score += score;
        updateText();
    }

    public void ResetScore()
    {
        this.score = 0;
        updateText();
    }

    void updateText()
    {
        tmp.text = $"<mspace=.6em>{score.ToString("D3")}</mspace>";
    }
}
