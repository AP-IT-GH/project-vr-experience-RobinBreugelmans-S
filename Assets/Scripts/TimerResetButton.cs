using UnityEngine;

public class TimerResetButton : MonoBehaviour
{
    [SerializeField] TimerController timerController;

    public void ResetTimer()
    {
        timerController.StartTimer();
    }
}
