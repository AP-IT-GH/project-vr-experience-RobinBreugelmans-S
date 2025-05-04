using UnityEngine;

namespace Assets.Scripts
{
    public class TargetScript : MonoBehaviour
    {
        [SerializeField] short targetValue = 1;
        [SerializeField] short minX = 0;
        [SerializeField] short minY = 0;
        [SerializeField] short minZ = 0;
        [SerializeField] short maxX = 0;
        [SerializeField] short maxY = 0;
        [SerializeField] short maxZ = 0;
        private float x = 0;
        private float y = 0;
        private float z = 0;

        void Start()
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            z = Random.Range(minZ, maxZ);
            transform.localPosition = new Vector3(x, y, z);
        }

        // Update is called once per frame
        void Update()
        {
            // maybe for future movement?
        }

        short Hit()
        {
            Destroy(this);
            return targetValue;
        }
    }
}