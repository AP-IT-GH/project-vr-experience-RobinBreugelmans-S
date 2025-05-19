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
        [SerializeField] GameObject aiGun; // the gun the ai is gonna use
        Quaternion og_roatation;
        GameObject hitObject;

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
            sensor.AddObservation(this.transform.forward); // own horisontal position
            sensor.AddObservation(this.transform.up); // own vertical position
            sensor.AddObservation(-aiGun.transform.right); // gun horisontal
            sensor.AddObservation(aiGun.transform.up); // gun vertical
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float horizontal = actions.ContinuousActions[0]; // left/right
            float vertical = actions.ContinuousActions[1]; // up/down

            // rotate the agent based on ml input
            this.transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
            this.transform.Rotate(Vector3.right, -vertical * rotationSpeed * Time.deltaTime);
            SetReward(-0.0001f); //small punishment that will be irrelevant if target is in view, see further, this will hopefully dicurage random steps

            // Collect all available sensors for raycasting that are located in the childeren from the agent object.
            // This way you can add and remove as fit without needing to adjust the script.
            RayPerceptionSensorComponent3D[] sensors = GetComponentsInChildren<RayPerceptionSensorComponent3D>();
            bool targetDetected = false;

            // Loop over all the sensors and check if one of the raycasts of that sensor found something
            foreach (RayPerceptionSensorComponent3D sensor in sensors)
            {
                RayPerceptionOutput output = RayPerceptionSensor.Perceive(sensor.GetRayPerceptionInput(), true);
                foreach (RayOutput ray in output.RayOutputs)
                {
                    // if it found something set the flag
                    if (ray.HasHit && ray.HitGameObject != null && ray.HitGameObject.layer == 6) // Check if the ray hit the target
                    {
                        hitObject = ray.HitGameObject;
                        targetDetected = true;
                        break;
                    }
                }
                if (targetDetected)
                {
                    break;
                }
            }


            if (targetDetected)
            {
                float angleBetweenAgentTarget = Vector3.Dot(-aiGun.transform.right, (hitObject.transform.position - aiGun.transform.position).normalized); // find the angel (from -1 to 1) between the gun and target, 1 should be strait before the gun
                if (angleBetweenAgentTarget > 0.8f) // do not reward random angles, only when truly close
                {
                    SetReward(angleBetweenAgentTarget * 0.3f); // Give a reward as long as the target is detected and is closet by the 1 degrees angle ( so if its strait before the gun).
                }
                else if (angleBetweenAgentTarget > 0.5f) // smaller reward for bigger angel, but still reasonable
                {
                    SetReward(angleBetweenAgentTarget * 0.1f);
                }
                else
                {
                    SetReward(angleBetweenAgentTarget * 0.001f);
                }
                if (Physics.Raycast(aiGun.transform.position, -aiGun.transform.right, out RaycastHit hit, fovDepth) && hit.transform.gameObject.layer == 6) // Direction is -right case thats how the gun prefab from the asset store is orientated
                {
                    // if the given target is hit by the gun the episode is done and the agent won
                    int score = hit.collider.GetComponent<TargetScript>().Hit();
                    ScoreScript.AddScoreML(score);
                    SetReward(1.5f);
                    EndEpisode();
                }
            }
            else
            {
                SetReward(-0.2f); // punishment for not finding target withing given steps
                EndEpisode();
            }
        }
    }
}