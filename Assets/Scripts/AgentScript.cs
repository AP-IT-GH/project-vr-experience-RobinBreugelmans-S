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
        [SerializeField] float rotationSpeed = 90f; // speed to rotate ML agent head to mach human speed
        Quaternion startRotation;

        private void Start()
        {
            startRotation = this.transform.rotation;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.localRotation);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            // check if target found
            if (this.RayCastPeripheralVision(out Vector3 directionToLook))
            {
                // rotate to target at human pace
                Quaternion rotateDirection = Quaternion.LookRotation(directionToLook);
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotateDirection, rotationSpeed * Time.deltaTime);
                // 'shoot' target
                Physics.Raycast(directionToLook, directionToLook, out RaycastHit hit, fovDepth);
                for (int i = 0; i < obstaclePrefabs.Length; i++)
                {
                    // if target hit, check tag to be sure
                    if (hit.collider.CompareTag(obstaclePrefabs[i].tag))
                    {
                        // add score from target to ml score
                        int score = hit.collider.gameObject.GetComponent<TargetScript>().Hit();
                        ScoreScript.AddScoreML(score);
                    }
                }
            }
            else
            {
                // if no target found, reset to begin position at human pace
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, startRotation, rotationSpeed * Time.deltaTime);
            }
        }

        public override void OnEpisodeBegin()
        {
            Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
        }

        // replicates complete human vision to find target
        private bool RayCastPeripheralVision(out Vector3 directionToLook)
        {
            float fovHorizontalCenter = fovHorizontal / 2;
            float fovVerticalCenter = fovVertical / 2; // center of fov for both vertical and horizontal
            Vector3 faceDirection = this.transform.forward; //get current direcction the agent is facing

            for (int i = 0; i < precisionAmountRays; i++)
            {
                // devide fov with the presisionAmountRays to slowly turn from side to side via the loop
                float angleHorizontal = Mathf.Lerp(-fovHorizontalCenter, fovHorizontalCenter, (float)i / (precisionAmountRays - 1));
                for (int j = 0; j < precisionAmountRays; j++)
                {
                    // devide fov with the presisionAmountRays to slowly turn from up to down via the loop
                    float angleVertical = Mathf.Lerp(-fovVerticalCenter, fovVerticalCenter, (float)j / (precisionAmountRays - 1));
                    // calculate direction to look and possible rotate
                    Vector3 direction = Quaternion.Euler(angleVertical, angleHorizontal, 0) * faceDirection;
                    // check if target at that direction
                    for (int k = 0; k < obstaclePrefabs.Length; k++)
                    {
                        if (Physics.Raycast(this.transform.position, direction, out RaycastHit hit, fovDepth) && hit.collider.CompareTag(obstaclePrefabs[k].tag))
                        {
                            // if target then finish looking
                            Debug.DrawLine(faceDirection, hit.point, Color.yellow);
                            directionToLook = direction;
                            return true;
                        }
                        else
                        {
                            Debug.DrawRay(faceDirection, direction * fovDepth, Color.gray);
                        }
                    }
                }
            }
            directionToLook = Vector3.zero;
            return false;
        }
    }
}