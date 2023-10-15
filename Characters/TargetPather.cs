using System;
using UnityEngine;
using UnityEngine.AI;

namespace Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class TargetPather : MonoBehaviour
    {
        [field: SerializeField] public NavMeshAgent Agent { get; set; }
        [Range(3f, 10f)] [SerializeField] private float interactRange = 3f;
        [SerializeField] private bool keepFollowing;
        [Range(0, 50f)] [SerializeField] private float kiteRange = 20f;

        public GameObject Target { get; private set; }
        public Action<GameObject> OnTargetReach { get; set; }
        public Action<Vector3> OnPositionReach { get; set; }

        private float _currentInteractRange;
        private Vector3? _targetPosition;

        private void Start()
        {
            _currentInteractRange = interactRange;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var position = transform.position;
            Gizmos.DrawWireSphere(position, interactRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, _currentInteractRange);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(position, kiteRange);
        }

        private void Update()
        {
            var targetPosition = _targetPosition ?? (Target ? Target.transform.position : null);

            if (targetPosition == null)
            {
                return;
            }

            if (Vector3.Distance(transform.position, targetPosition.Value) > _currentInteractRange)
            {
                if (Target && kiteRange != 0 && Vector3.Distance(transform.position, targetPosition.Value) > kiteRange)
                {
                    Debug.Log(this + " lost " + Target);
                    ClearTarget();
                }
                else
                {
                    Agent.SetDestination(targetPosition.Value);
                }
            }
            else
            {
                FaceTarget(targetPosition.Value);
                _currentInteractRange = interactRange;
                if (Agent.hasPath)
                {
                    Agent.ResetPath();
                }

                if (OnTargetReach != null && Target)
                {
                    OnTargetReach(Target);

                }
                if (OnPositionReach != null && _targetPosition.HasValue)
                {
                    OnPositionReach(_targetPosition.Value);
                }

                if (!keepFollowing)
                {
                    ClearTarget();
                }
            }
        }

        public void ClearTarget(bool resetPath = true)
        {
            Target = null;
            _targetPosition = null;
            if (resetPath)
            {
                Agent.ResetPath();
            }
        }

        private void FaceTarget(Vector3 targetPosition)
        {
            var lookPos = targetPosition - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);
        }

        public void SetTarget(GameObject target, float? range = null)
        {
            Target = target;
            _currentInteractRange = range ?? interactRange;
        }

        public void SetDestination(Vector3 destination, float range = 0.1f)
        {
            _targetPosition = destination;
            _currentInteractRange = range;
        }
    }
}
