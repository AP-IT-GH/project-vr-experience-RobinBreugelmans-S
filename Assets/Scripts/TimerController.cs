using UnityEngine;
using TMPro;
using Assets.Scripts;

public class TimerController : MonoBehaviour
{
    public float timerDuration = 60f;
    private float timer;
    public bool TimerRunning { get; private set; } = false;

    private TextMeshPro text; // Text component reference
    [SerializeField] private ScoreScript scoreScript;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        timer = timerDuration;
        UpdateText();
    }

    public void StartTimer()
    {
        timer = timerDuration;
        TimerRunning = true;

        if (scoreScript != null)
        {
            scoreScript.ResetScores();
        }
        /*else
        {
            Debug.LogWarning("ScoreScript not assigned in TimerController.");
        }*/
    }

    void Update()
    {
        if (TimerRunning && !PauseManager.Instance.IsPaused)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                TimerRunning = false;
            }
            UpdateText();
        }
    }

    void UpdateText()
    {
        if (text != null)
        {
            text.text = timer.ToString("F1"); // Show one decimal place
        }
    }
}
