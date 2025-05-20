using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class TargetControllerScript : MonoBehaviour
    {
        [SerializeField] GameObject[] obstaclePrefabs; // the targets to look for

        [SerializeField] float minX = -6;
        [SerializeField] float minY = -2;
        [SerializeField] float minZ = -5;
        [SerializeField] float maxX = 6;
        [SerializeField] float maxY = 1;
        [SerializeField] float maxZ = -25;
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
                Quaternion.Euler(0, 0, 0) // 90° Z rotation
                );
            this.GetComponentInChildren<AgentScript>().SetNewTarget(currentTarget);
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
                    Quaternion.Euler(0, 0, 0) // 90° Z rotation
                    );
                this.GetComponentInChildren<AgentScript>().SetNewTarget(currentTarget);
            }
        }
    }
}