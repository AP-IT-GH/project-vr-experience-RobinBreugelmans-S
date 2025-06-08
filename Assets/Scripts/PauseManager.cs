using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; } = false;

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
        Debug.Log($"Pause Toggled. IsPaused = {IsPaused}");
    }

    public void SetPause(bool value)
    {
        IsPaused = value;
    }
}