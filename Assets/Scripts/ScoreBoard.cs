using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    TextMeshPro tmp;
    short score = 0;

    public void AddScore(short score)
    {
        this.score += score;
        updateText();
    }

    void updateText()
    {
        tmp.text = $"<mspace=.6em>{score.ToString("D3")}</mspace>";
    }
}
