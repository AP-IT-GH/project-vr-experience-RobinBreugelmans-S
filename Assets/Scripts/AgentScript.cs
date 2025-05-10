using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using static Unity.MLAgents.Sensors.RayPerceptionOutput;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class AgentScript : Agent
    {
        [SerializeField] GameObject[] obstaclePrefabs; // the targets to look for
        [SerializeField] short fovDepth = 10; // max distance to look at
        [SerializeField] float rotationSpeed = 90f; // speed to rotate ML agent head to mach human speed
        GameObject target;
        Quaternion og_rotation; // to store initial rotation to compare its current rotation to and give penalty when rotating out of gunrange

        private void Start()
        {
            og_rotation = this.transform.rotation;
        }
        public override void OnEpisodeBegin()
        {
            if (target != null)
            {
                Destroy(target);
            }
            target = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)]);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.forward); // own forward position
            sensor.AddObservation(this.transform.rotation); // own rotation
            Vector3 og_forward = og_rotation * Vector3.forward;
            float alignment = Vector3.Dot(this.transform.forward, og_forward);
            sensor.AddObservation(alignment); // value between -1 and 1
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float horizontal = actions.ContinuousActions[0];   // left/right
            float vertical = actions.ContinuousActions[1]; // up/down

            // rotate the agent based on ml input
            this.transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
            this.transform.Rotate(Vector3.right, -vertical * rotationSpeed * Time.deltaTime);


            // Soft rotation penalty based on deviation from original forward
            Vector3 og_forward = og_rotation * Vector3.forward;
            float alignment = Vector3.Dot(this.transform.forward.normalized, og_forward.normalized); // looks for difference in angle and puts it between -1 and 1
            float anglePenalty = 1f - Mathf.Clamp01((alignment + 1f) / 2f); // Penalty increases as misalignment grows
            AddReward(-anglePenalty * 0.005f);

            // Reward for aligning with original forward direction
            float angleReward = Mathf.Clamp01((alignment + 1f) / 2f); // reward increases as alligment grows
            AddReward(angleReward * 0.005f);


            RayPerceptionSensorComponent3D[] sensors = GetComponentsInChildren<RayPerceptionSensorComponent3D>();
            bool targetDetected = false;

            foreach (RayPerceptionSensorComponent3D sensor in sensors){
                RayPerceptionOutput output = RayPerceptionSensor.Perceive(sensor.GetRayPerceptionInput(), true);
                foreach (RayOutput ray in output.RayOutputs)
                {
                    if (ray.HasHit && ray.HitGameObject != null && ray.HitGameObject.layer == 6) // Check if the ray hit the target
                    {
                        targetDetected = true;
                        break;
                    }
                }
            }
            

            if (targetDetected)
            {
                AddReward(0.1f); // Reward agent for keeping the target in sight
            }

            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, fovDepth) && hit.transform.gameObject.layer == 6)
            {
                Shoot();
                AddReward(2.0f);
                EndEpisode();
            }

            // useless step punishment
            AddReward(-0.001f);
            
            if (StepCount > 500)
            {
                AddReward(-1f); // punishment for not finding target withing given steps
                EndEpisode();
            }
        }

        void Shoot()
        {
            int score = this.GetComponentInChildren<Gun>().Shoot();
            ScoreScript.AddScoreML(score);

            //if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, fovDepth))
            //{
            //    int score = hit.collider.GetComponent<TargetScript>().Hit();
            //    ScoreScript.AddScoreML(score);
            //}
        }
    }
}