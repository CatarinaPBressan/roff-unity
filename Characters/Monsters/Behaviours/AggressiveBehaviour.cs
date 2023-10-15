using Characters.Players;
using UnityEngine;

namespace Characters.Monsters.Behaviours
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(TargetPather))]
    public class AggressiveBehaviour : MonoBehaviour
    {
        [SerializeField] private TargetPather targetPather;
        [Range(3f, 50f)] [SerializeField] private float aggressiveRange;
        private GameObject[] _players;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggressiveRange);
        }

        private void Start()
        {
            targetPather = GetComponent<TargetPather>();
            _players = GameObject.FindGameObjectsWithTag("Player");
        }

        private void Update()
        {
            if (!targetPather.Target)
            {
                foreach (var player in _players)
                {
                    if (!player.TryGetComponent<PlayerAttributes>(out var playerAttributes) ||
                        playerAttributes.IsDead ||
                        !(Vector3.Distance(transform.position, player.transform.position) <= aggressiveRange))
                    {
                        continue;
                    }
                    targetPather.SetTarget(player);
                    Debug.Log( this + "acquired "+ player + " as a target!");
                    break;
                }
            }
            else if (targetPather.Target.TryGetComponent<PlayerAttributes>(out var playerAttributes) && playerAttributes.IsDead)
            {
                targetPather.ClearTarget();
            }
        }
    }
}
