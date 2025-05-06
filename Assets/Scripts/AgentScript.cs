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
        GameObject target;

        private void Start()
        {
            startRotation = this.transform.rotation;
        }
        public override void OnEpisodeBegin()
        {
            target = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.forward);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float horizontal = actions.ContinuousActions[0];   // left/right
            float vertical = actions.ContinuousActions[1]; // up/down
            bool targetFound = false;

            // let agent rotate
            Quaternion rotate = Quaternion.LookRotation(Vector3.up * horizontal + Vector3.right * vertical);
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotate, rotationSpeed * Time.deltaTime);


            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, fovDepth))
            {
                for (int i = 0; i < obstaclePrefabs.Length; i++) {
                    if (hit.collider.CompareTag(obstaclePrefabs[i].tag))
                    {
                        Vector3 directionToTarget = (hit.point - this.transform.position).normalized;
                        // Calculate the angle between agent's forward direction and target direction (horizontal check)
                        float horizontalAngle = Vector3.Angle(new Vector3(this.transform.forward.x, 0, this.transform.forward.z), new Vector3(directionToTarget.x, 0, directionToTarget.z));

                        // Calculate the vertical angle (check for vertical FOV)
                        float verticalAngle = Vector3.Angle(new Vector3(0, this.transform.forward.y, 0), new Vector3(0, directionToTarget.y, 0));

                        // Check if the target is within the horizontal FOV
                        if (horizontalAngle < fovHorizontal / 2f && verticalAngle < fovVertical / 2f)
                        {
                            Quaternion rotationToTarget = Quaternion.LookRotation(hit.transform.position);
                            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotationToTarget, rotationSpeed * Time.deltaTime);
                            Shoot(hit);
                            targetFound = true;
                            SetReward(2.0f);
                        }
                    }
                }
            }
            if (!targetFound)
            {
                SetReward(-0.1f);
            }
        }

        void Shoot(RaycastHit hit)
        {
            //TODO: replace the hit with an actual shot from gun (Robin's code)
            int score = hit.collider.GetComponent<TargetScript>().Hit();
            ScoreScript.AddScoreML(score);
        }

        //public override void OnActionReceived(ActionBuffers actions)
        //{
        //    // check if target found
        //    if (this.RayCastPeripheralVision(out Vector3 directionToLook))
        //    {
        //        // rotate to target at human pace
        //        Quaternion rotateDirection = Quaternion.LookRotation(directionToLook);
        //        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotateDirection, rotationSpeed * Time.deltaTime);
        //        // 'shoot' target
        //        Physics.Raycast(directionToLook, directionToLook, out RaycastHit hit, fovDepth);
        //        for (int i = 0; i < obstaclePrefabs.Length; i++)
        //        {
        //            // if target hit, check tag to be sure
        //            if (hit.collider.CompareTag(obstaclePrefabs[i].tag))
        //            {
        //                // add score from target to ml score
        //                int score = hit.collider.gameObject.GetComponent<TargetScript>().Hit();
        //                ScoreScript.AddScoreML(score);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // if no target found, reset to begin position at human pace
        //        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, startRotation, rotationSpeed * Time.deltaTime);
        //    }
        //}

        //// replicates complete human vision to find target
        //private bool RayCastPeripheralVision(out Vector3 directionToLook)
        //{
        //    float fovHorizontalCenter = fovHorizontal / 2;
        //    float fovVerticalCenter = fovVertical / 2; // center of fov for both vertical and horizontal
        //    Vector3 faceDirection = this.transform.forward; //get current direcction the agent is facing

        //    for (int i = 0; i < precisionAmountRays; i++)
        //    {
        //        // devide fov with the presisionAmountRays to slowly turn from side to side via the loop
        //        float angleHorizontal = Mathf.Lerp(-fovHorizontalCenter, fovHorizontalCenter, (float)i / (precisionAmountRays - 1));
        //        for (int j = 0; j < precisionAmountRays; j++)
        //        {
        //            // devide fov with the presisionAmountRays to slowly turn from up to down via the loop
        //            float angleVertical = Mathf.Lerp(-fovVerticalCenter, fovVerticalCenter, (float)j / (precisionAmountRays - 1));
        //            // calculate direction to look and possible rotate
        //            Vector3 direction = Quaternion.Euler(angleVertical, angleHorizontal, 0) * faceDirection;
        //            // check if target at that direction
        //            for (int k = 0; k < obstaclePrefabs.Length; k++)
        //            {
        //                if (Physics.Raycast(this.transform.position, direction, out RaycastHit hit, fovDepth) && hit.collider.CompareTag(obstaclePrefabs[k].tag))
        //                {
        //                    // if target then finish looking
        //                    Debug.DrawLine(faceDirection, hit.point, Color.yellow);
        //                    directionToLook = direction;
        //                    return true;
        //                }
        //                else
        //                {
        //                    Debug.DrawRay(faceDirection, direction * fovDepth, Color.gray);
        //                }
        //            }
        //        }
        //    }
        //    directionToLook = Vector3.zero;
        //    return false;
        //}
    }
}