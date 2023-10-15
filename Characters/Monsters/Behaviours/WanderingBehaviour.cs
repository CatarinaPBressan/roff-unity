using UnityEngine;
using UnityEngine.AI;

using Utils;
using Random = UnityEngine.Random;

namespace Monsters.Behaviours
{

    [RequireComponent(typeof(NavMeshAgent))]
    public class WanderingBehaviour : MonoBehaviour
    {
        [Range(1, 50f)] [SerializeField] private float wanderRange = 10f;
        [SerializeField] private NavMeshAgent agent;
        [Range(0.01f, 1f)] [SerializeField] private float wanderChance = 0.5f;
        [Range(0.5f, 5f)] [SerializeField] private float wanderCooldown = 1f;

        private ActionTimer _timer;

        private void Start()
        {
            _timer = new ActionTimer(wanderCooldown, Wander);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, wanderRange);
        }

        void Update()
        {
            if (!agent.hasPath)
            {
                _timer.Elapse(Time.deltaTime);
            }
            else
            {
                _timer.Reset();
            }
        }

        void Wander()
        {
            if (Random.value <= wanderChance)
            {
                var pathPoint = NavMeshUtils.GetRandomNavMeshPoint(transform.position, wanderRange);
                if (pathPoint.HasValue)
                {
                    agent.destination = pathPoint.Value;
                }
            }
        }
    }
}
