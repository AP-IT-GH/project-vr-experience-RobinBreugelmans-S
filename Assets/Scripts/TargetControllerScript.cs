using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class TargetControllerScript : MonoBehaviour
    {
        [SerializeField] GameObject[] obstaclePrefabs; // the targets to look for
        [SerializeField] bool trainingMode = true; // do not spawn targets while training, agent does it itself
        GameObject currentTarget;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (!trainingMode)
            {
                currentTarget = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!currentTarget && !trainingMode)
            {
                currentTarget = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
            }
        }
    }
}