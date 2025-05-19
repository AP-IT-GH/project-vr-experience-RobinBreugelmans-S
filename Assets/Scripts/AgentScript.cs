using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Assets.Scripts
{
    public class AgentScript : Agent
    {
        [SerializeField] GameObject aiGun;
        [SerializeField] float rotationSpeed = 50.0f;
        [SerializeField] LayerMask layerOfTarget;
        [SerializeField] float fovDistance = 90f;
        GameObject target;
        int maxShots = 20;
        int currentShotCount = 0;

        public override void OnEpisodeBegin()
        {
            this.transform.localRotation = Quaternion.Euler(0, -90, 0);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.localRotation.normalized); // agent can know its own rotation
            sensor.AddObservation((-aiGun.transform.right).normalized); // and the front of the gun
            if (target != null)
            {
                sensor.AddObservation(target.transform);
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float horizontal = actions.ContinuousActions[0];
            int shoot = actions.DiscreteActions[0];

            transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);
            SetReward(-0.001f);

            if (shoot == 1 && currentShotCount < maxShots)
            {
                currentShotCount++;
                SetReward(-0.01f);
                if (Physics.Raycast(aiGun.transform.position, -aiGun.transform.right, out RaycastHit rayHit, fovDistance) && rayHit.transform.gameObject.layer == layerOfTarget)
                {
                    SetReward(2.0f);
                    ScoreScript.AddScoreML(rayHit.transform.gameObject.GetComponent<TargetScript>().Hit());
                    EndEpisode();
                }
            }
            else if (shoot != 0 && currentShotCount < maxShots)
            {
                SetReward(0.01f);
            }
            else
            {
                SetReward(-1.0f);
                EndEpisode();
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
        }

        public void SetNewTarget(GameObject target)
        {
            this.target = target;
        }
    }
}
