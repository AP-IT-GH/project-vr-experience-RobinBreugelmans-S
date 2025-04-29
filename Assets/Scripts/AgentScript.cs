using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace Assets.Scripts
{
    public class AgentScript : Agent
    {
        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(this.transform.localRotation);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            base.OnActionReceived(actions);
        }

        public override void OnEpisodeBegin()
        {
            
        }
    }
}