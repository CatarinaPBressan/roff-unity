using System;
using System.Collections.Generic;
using Characters.Skills;
using UnityEngine;

namespace Characters
{
    public class CharacterAttributes : MonoBehaviour
    {
        protected int CurrentHp = 1;
        public int CurrentSp { get; set; } = 1;

        protected List<Skill> Skills;

        protected virtual void Start()
        {
            Skills = new List<Skill>();
        }

        protected virtual void OnDeath(CharacterAttributes attackerAttributes) {}
        protected virtual void OnDamageTaken(CharacterAttributes attackerAttributes) {}
        public virtual int GetCharacterPhysicalDamage(){throw new NotImplementedException();}

        public void BeDamagedByAttacker(CharacterAttributes attackerAttributes, int damage)
        {
            var newHp = CurrentHp - damage;
            CurrentHp = newHp <= 0 ? 0 : newHp;

            Debug.Log(this + " took " + damage + " damage. Now has " + CurrentHp + " HP");

            if (CurrentHp == 0)
            {
                OnDeath(attackerAttributes);
                Debug.Log(this + " has died!");
            }
            else
            {
                OnDamageTaken(attackerAttributes);
            }
        }

        public void BeDamagedByAttacker(CharacterAttributes attackerAttributes)
        {
            BeDamagedByAttacker(attackerAttributes, attackerAttributes.GetCharacterPhysicalDamage());
        }

        public virtual int GetCharacterMagicalDamage(){throw new NotImplementedException();}
    }
}
