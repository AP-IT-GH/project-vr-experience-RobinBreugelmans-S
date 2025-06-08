using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class TargetScript : MonoBehaviour
    {
        [SerializeField] short points = 1;

        public short Hit()
        {
            if (!PauseManager.Instance.IsPaused)
            {
                Destroy(this.transform.parent.gameObject); // destroy the parent of the individual target circles
                return points;
            }
            else
            {
                return 0;
            }
            
        }
    }
}