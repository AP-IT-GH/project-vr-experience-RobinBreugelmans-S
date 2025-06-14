﻿using Unity.MLAgents;
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
        [SerializeField] TimerController timerController;
        GameObject target;
        int maxShots = 20;
        int currentShotCount = 0;
        Animator animator;
        [SerializeField] ScoreScript score;
        [SerializeField] AudioSource shotSound;


        void Start()
        {
            // Get the Animator from a child GameObject
            animator = GetComponentInChildren<Animator>();
            Transform deagle = transform.Find("Deagle"); // get gun to make invisible
            if (deagle != null)
            {
                Renderer deagleRenderer = deagle.GetComponent<Renderer>();
                if (deagleRenderer != null)
                {
                    deagleRenderer.enabled = false;  // Makes Deagle invisible
                }
            }
        }

        public override void OnEpisodeBegin()
        {
            this.transform.localRotation = Quaternion.Euler(0, -90, 0);
            this.currentShotCount = 0;
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
            if (!timerController.TimerRunning || PauseManager.Instance.IsPaused)
            {
                // AI should do nothing if timer isn't running
                return;
            }
            float horizontal = actions.ContinuousActions[0];
            float shoot = actions.ContinuousActions[1];
            //Debug.Log($"shoot: {shoot}, horizontal: {horizontal}");

            transform.Rotate(Vector3.up * horizontal * rotationSpeed * Time.deltaTime);
            SetReward(-0.001f);

            if (shoot >= 0.5f && currentShotCount < maxShots)
            {
                shotSound.Play();
                Debug.Log("Shooting");
                currentShotCount++;
                SetReward(0.001f);
                Debug.DrawRay(aiGun.transform.position, -aiGun.transform.right*fovDistance, Color.red);
                animator.SetTrigger("Shoot");
                if (Physics.Raycast(aiGun.transform.position, -aiGun.transform.right, out RaycastHit hit, fovDistance) && hit.transform.gameObject.layer == 6)
                {
                    Debug.Log("Hit");
                    SetReward(2.0f);
                    score.AddScoreML(hit.transform.gameObject.GetComponent<TargetScript>().Hit());
                    EndEpisode();
                }
            }
            else if (shoot < 0.5f && currentShotCount < maxShots)
            {
                SetReward(-0.01f);
            }
            else
            {
                SetReward(-1.0f);
                EndEpisode();
            }
        }

        public void SetNewTarget(GameObject target)
        {
            this.target = target;
        }
    }
}
