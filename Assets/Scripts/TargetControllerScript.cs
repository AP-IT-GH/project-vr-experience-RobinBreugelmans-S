using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class TargetControllerScript : MonoBehaviour
    {
        [SerializeField] GameObject[] obstaclePrefabs; // the targets to look for

        [SerializeField] short targetValue = 1;
        [SerializeField] float minX = -5;
        [SerializeField] float minY = 0;
        [SerializeField] float minZ = 0;
        [SerializeField] float maxX = 5;
        [SerializeField] float maxY = 5;
        [SerializeField] float maxZ = 0;
        private float x = 0;
        private float y = 0;
        private float z = 0;

        GameObject currentTarget;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            x = Random.Range(minX, maxX);
            y = Random.Range(minY, maxY);
            z = Random.Range(minZ, maxZ);
            currentTarget = Instantiate(
                obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], 
                position: new Vector3(x, y, z), 
                Quaternion.Euler(0, 0, 90) // 90° Z rotation
                );
        }

        // Update is called once per frame
        void Update()
        {
            if (currentTarget == null)
            {
                x = Random.Range(minX, maxX);
                y = Random.Range(minY, maxY);
                z = Random.Range(minZ, maxZ);
                currentTarget = Instantiate(
                    obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)],
                    position: new Vector3(x, y, z),
                    Quaternion.Euler(0, 0, 90) // 90° Z rotation
                    );
            }
        }
    }
}