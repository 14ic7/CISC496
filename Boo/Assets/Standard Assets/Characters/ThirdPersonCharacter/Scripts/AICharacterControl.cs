using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target { get; set; } // target to aim for
		Vector3 _destination; // location to aim for


		private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
			if (agent.enabled) {
				if (target != null) {
					agent.SetDestination (target.position);
				} else {
					agent.SetDestination (_destination);
				}

				if (agent.remainingDistance > agent.stoppingDistance) {
					character.Move (agent.desiredVelocity, false, false);
				} else {
					character.Move (Vector3.zero, false, false);
				}
			}
        }


		public void SetDestination(Vector3 destination) {
			target = null;
			_destination = destination;
		}

		public void Stop() {
			Debug.Log("Stop");
			SetDestination(transform.position);
		}

		public void Pause() { agent.Stop(); }
		public void Resume() { agent.Resume(); }
    }
}
