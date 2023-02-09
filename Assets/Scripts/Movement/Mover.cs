using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavMapthLenght = 40f;

        Health health;

        NavMeshAgent navMeshAgent;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            
            UpdateAnimator();
        }


        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLenght(path) > maxNavMapthLenght) return false;

            return true;
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardspeed", speed);
        }

        private float GetPathLenght(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        public object CaptureState()
        {
            // capturing several pieces of data, using a Dictionary
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
            
            // return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;  // this doesn't exist in his final script
            //SerializableVector3 position = (SerializableVector3)state;
            navMeshAgent.enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();    // this doesn't exist in his final script
            //transform.position = position.ToVector();
            navMeshAgent.enabled = true;
            //GetComponent<ActionScheduler>().CancelCurrentAction();



        }

        // Another approach to save several pieces of data, using a struct:

        // [System.Serializable]
        // struct MoverSaveData
        // {
        //     public SerializableVector3 position;-
        //     public SerializableVector3 rotation;
        // }

        // public object CaptureState()
        // {
        //     MoverSaveData data = new MoverSaveData();
        //     data.position = new SerializableVector3(transform.position);
        //     data.rotation = new SerializableVector3(transform.eulerAngles);
        //     return data;
        // }

        // public void RestoreState(object state)
        // {
        //     MoverSaveData data = (MoverSaveData)state;
        //     GetComponent<NavMeshAgent>().enabled = false;
        //     transform.position = data.position.ToVector();
        //     transform.eulerAngles = data.rotation.ToVector();
        //     GetComponent<NavMeshAgent>().enabled = true;
        // }

    }
}


