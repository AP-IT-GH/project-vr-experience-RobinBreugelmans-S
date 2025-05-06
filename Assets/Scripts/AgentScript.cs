using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace Assets.Scripts
{
    public class AgentScript : Agent
    {
        [SerializeField] GameObject[] obstaclePrefabs; // the targets to look for
        [SerializeField] short fovDepth = 10; // max distance to look at
        [SerializeField] float fovHorizontal = 220f; // human horisontal fov
        [SerializeField] float fovVertical = 135f; // human vertical fov
        [SerializeField] short precisionAmountRays = 30; // for the presision of finding a target

        private GameObject target;
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.localRotation);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            Vector3 directionToLook = this.RayCastPeripheralVision();
            this.transform.rotation = Quaternion.LookRotation(directionToLook);
            Physics.Raycast(directionToLook, directionToLook, out RaycastHit hit, fovDepth);
            for (int i = 0; i <= obstaclePrefabs.Length; i++)
            {
                if (hit.collider.CompareTag(obstaclePrefabs[i].tag))
                {
                    int score = hit.collider.gameObject.GetComponent<TargetScript>().Hit();

                }
            }
        }

        public override void OnEpisodeBegin()
        {
            target = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
        }

        // replicates complete human vision to find target
        private Vector3 RayCastPeripheralVision()
        {
            float fovHorizontalCenter = fovHorizontal / 2;
            float fovVerticalCenter = fovVertical / 2;
            Vector3 faceDirection = this.transform.localPosition;

            for (int i = 0; i < precisionAmountRays; i++)
            {
                // devide fov with the presisionAmountRays to slowly turn from side to side via the loop
                float angleHorizontal = Mathf.Lerp(-fovHorizontalCenter, fovHorizontalCenter, (float)i / (precisionAmountRays - 1));
                for (int j = 0; j < precisionAmountRays; j++)
                {
                    // devide fov with the presisionAmountRays to slowly turn from up to down via the loop
                    float angleVertical = Mathf.Lerp(-fovVerticalCenter, fovVerticalCenter, (float)j / (precisionAmountRays - 1));

                    Vector3 direction = Quaternion.Euler(angleVertical, angleHorizontal, 0) * faceDirection;
                    this.transform.rotation = Quaternion.LookRotation(direction); // move with looking to make realistic
                    if (Physics.Raycast(faceDirection, direction, out RaycastHit hit, fovDepth) && hit.collider.CompareTag("target"))
                    {
                        Debug.DrawLine(faceDirection, hit.point, Color.yellow);
                        return direction;
                    }
                    else
                    {
                        Debug.DrawRay(faceDirection, direction * fovDepth, Color.gray);
                    }
                }
            }
            return faceDirection;
        }
    }
}