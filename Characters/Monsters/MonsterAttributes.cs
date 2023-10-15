using System;
using Characters.Players;
using UnityEngine;

namespace Characters.Monsters
{
    public class MonsterAttributes: CharacterAttributes
    {
        [SerializeField] private int maxHp = 153;
        [SerializeField] private int attackDamage = 33;
        [SerializeField] private int baseExp = 108;
        [SerializeField] private int jobExp = 81;

        private MonsterController _monsterController;

        protected override void Start()
        {
            base.Start();

            _monsterController = GetComponent<MonsterController>();

            CurrentHp = maxHp;
            CurrentSp = 0;
        }

        public override int GetCharacterPhysicalDamage()
        {
            return attackDamage;
        }

        public override int GetCharacterMagicalDamage()
        {
            return (int)Math.Floor(attackDamage / 2f);
        }

        protected override void OnDeath(CharacterAttributes attackerAttributes)
        {
            var playerAttributes = (PlayerAttributes)attackerAttributes;

            playerAttributes.AddBaseExp(baseExp);
            playerAttributes.AddJobExp(jobExp);

            _monsterController.OnDeath(playerAttributes);
        }

        protected override void OnDamageTaken(CharacterAttributes attackerAttributes)
        {
            _monsterController.OnDamageTaken((PlayerAttributes)attackerAttributes);
        }
    }
}
