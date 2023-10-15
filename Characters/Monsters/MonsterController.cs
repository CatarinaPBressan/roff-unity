using System;
using Characters.Players;
using UnityEngine;
using Utils;

namespace Characters.Monsters
{
    [RequireComponent(typeof(MonsterAttributes))]
    [RequireComponent(typeof(MonsterStats))]
    public class MonsterController : NonPlayerCharacter
    {
        [SerializeField] private TargetPather targetPather;

        private CooldownTimer _attackCoolDownTimer;
        private MonsterStats _monsterStats;
        private MonsterAttributes _monsterAttributes;

        private void Start()
        {
            CursorTexture = (Texture2D)Resources.Load("Cursors/sword");
            if (targetPather)
            {
                targetPather.OnTargetReach = Attack;
            }

            _monsterAttributes = GetComponent<MonsterAttributes>();
            _monsterStats = GetComponent<MonsterStats>();

            _attackCoolDownTimer = new CooldownTimer();
        }

        public void Update()
        {
            _attackCoolDownTimer.Elapse(Time.deltaTime);
        }

        private void Attack(GameObject target)
        {
            if (!target.TryGetComponent<PlayerAttributes>(out var playerAttributes) ||
                !_attackCoolDownTimer.IsReady())
            {
                return;
            }

            playerAttributes.BeDamagedByAttacker(_monsterAttributes);
            _attackCoolDownTimer.Start(1);
        }

        public void ClearTarget()
        {
            if (targetPather)
            {
                targetPather.ClearTarget();
            }
        }

        public void OnDeath(PlayerAttributes attackerAttributes)
        {
            ClearCursor();

            OnMonsterDeath(new MonsterDeathEventArgs
            {
                MonsterGameObject = gameObject
            });

            Destroy(gameObject);
        }

        public void OnDamageTaken(PlayerAttributes attackerAttributes)
        {
            if (targetPather)
            {
                targetPather.SetTarget(attackerAttributes.gameObject);
            }
        }

        public event EventHandler<MonsterDeathEventArgs> MonsterDeath;

        private void OnMonsterDeath(MonsterDeathEventArgs e)
        {
            MonsterDeath?.Invoke(this, e);
        }
    }
}
