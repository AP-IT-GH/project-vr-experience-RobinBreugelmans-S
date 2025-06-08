using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; } = false;

    [SerializeField] private GameObject pauseSign; // Assign in Inspector

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        UpdatePauseState();
    }

    public void SetPause(bool value)
    {
        IsPaused = value;
        UpdatePauseState();
    }

    private void UpdatePauseState()
    {
        if (pauseSign != null)
        {
            pauseSign.SetActive(IsPaused);
        }
    }
}
