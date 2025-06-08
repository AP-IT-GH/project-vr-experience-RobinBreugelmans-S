using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInputHandler : MonoBehaviour
{
    private XRControls controls;

    void Awake()
    {
        controls = new XRControls();
    }

    void OnEnable()
    {
        controls.Game.Pause.performed += OnPausePressed;
        controls.Game.Enable();
    }

    void OnDisable()
    {
        controls.Game.Pause.performed -= OnPausePressed;
        controls.Game.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        PauseManager.Instance.TogglePause();
    }
}
