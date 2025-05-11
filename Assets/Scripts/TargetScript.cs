using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class TargetScript : MonoBehaviour
    {
        [SerializeField] short points = 1;

        public short Hit()
        {
            Destroy(this.gameObject);
            return points;
        }
    }
}