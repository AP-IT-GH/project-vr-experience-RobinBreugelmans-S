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
        Quaternion og_roatation;

        void Start()
        {
            og_roatation = this.transform.rotation;
        }

        public override void OnEpisodeBegin()
        {
            this.transform.rotation = og_roatation; // reset rotation so it doesnt learn random positions
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.forward); // own forward position
            sensor.AddObservation(this.transform.rotation); // own rotation
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float horizontal = actions.ContinuousActions[0];   // left/right
            float vertical = actions.ContinuousActions[1]; // up/down

            // rotate the agent based on ml input
            this.transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
            this.transform.Rotate(Vector3.right, -vertical * rotationSpeed * Time.deltaTime);

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
                SetReward(0.5f); // Reward agent for keeping the target in sight
                if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, fovDepth) && hit.transform.gameObject.layer == 6)
                {
                    Shoot();
                    SetReward(2.0f);
                    EndEpisode();
                }
            }
            else
            {
                SetReward(-1f); // punishment for not finding target withing given steps
                EndEpisode();
            }
        }

        void Shoot()
        {
            //int score = this.GetComponentInChildren<Gun>().Shoot();
            //ScoreScript.AddScoreML(score);

            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, fovDepth))
            {
                int score = hit.collider.GetComponent<TargetScript>().Hit();
                ScoreScript.AddScoreML(score);
            }
        }
    }
}