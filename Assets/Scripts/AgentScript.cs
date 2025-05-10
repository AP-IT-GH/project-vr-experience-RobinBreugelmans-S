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
            sensor.AddObservation(this.transform.forward); // own position
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float horizontal = actions.ContinuousActions[0];   // left/right
            float vertical = actions.ContinuousActions[1]; // up/down
            bool targetFound = false;

            // rotate the agent based on ml input
            this.transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
            this.transform.Rotate(Vector3.right, -vertical * rotationSpeed * Time.deltaTime);

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
                            Shoot();
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

        void Shoot()
        {
            //TODO: replace the hit with an actual shot from gun (Robin's code)
            //this.GetComponentInChildren<Gun>().shoot();
            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, fovDepth))
            {
                int score = hit.collider.GetComponent<TargetScript>().Hit();
                ScoreScript.AddScoreML(score);
            }
        }
    }
}