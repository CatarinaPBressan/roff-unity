using System;
using Enums;
using UnityEngine;

namespace Characters.Skills
{
    public class Skill
    {
        protected readonly int SkillLevel;
        public SkillTargetType SkillTargetType = SkillTargetType.SingleTarget;

        protected Skill(int skillLevel)
        {
            SkillLevel = skillLevel;
        }

        public string Name { get; protected set; }

        public virtual float? Range()
        {
            throw new NotImplementedException();
        }

        protected virtual int? Damage(CharacterAttributes characterAttributes)
        {
            throw new NotImplementedException();
        }

        protected virtual int SpCost()
        {
            throw new NotImplementedException();
        }

        protected virtual float? AoERadius()
        {
            return null;
        }

        public void CastSkill(CharacterAttributes caster, CharacterAttributes target, PhysicsLayers areaOfEffectMask)
        {
            Debug.Log("casting " + Name + " on " + target.gameObject);
            if (!ProcessSpRequirement(caster))
            {
                return;
            }

            var skillDamage = Damage(caster);
            if (!skillDamage.HasValue)
            {
                //TODO: change this for skills with no damage (buffs, etc)
                return;
            }

            var aoeRadius = AoERadius();
            if (aoeRadius.HasValue)
            {
                ProcessSkillCastInRadius(caster, target.transform.position, aoeRadius.Value, skillDamage.Value, areaOfEffectMask);
            }
            else
            {
                target.BeDamagedByAttacker(caster, skillDamage.Value);
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private void ProcessSkillCastInRadius(CharacterAttributes caster,
            Vector3 position,
            float aoeRadius,
            int skillDamage,
            PhysicsLayers areaOfEffectMask)
        {
            DebugExtension.DebugWireSphere(position, Color.red, aoeRadius, 2f);
            // ReSharper disable once Unity.PreferNonAllocApi
            foreach (var collider in Physics.OverlapSphere(position, aoeRadius, (int)areaOfEffectMask))
            {
                if (!collider.TryGetComponent<CharacterAttributes>(out var characterAttributes))
                {
                    continue;
                }

                Debug.Log(characterAttributes);
                characterAttributes.BeDamagedByAttacker(caster, skillDamage);
            }
        }

        public void CastSkill(CharacterAttributes caster, Vector3 position, PhysicsLayers areaOfEffectMask)
        {
            Debug.Log("casting " + Name + " on " + position);
            var aoeRadius = AoERadius();
            if (aoeRadius is null or <= 0f)
            {
                throw new InvalidOperationException("Ground skill " + this + " is missing value for radius");
            }
            var skillDamage = Damage(caster);
            if (!skillDamage.HasValue)
            {
                //TODO: change this for skills with no damage (buffs, etc)
                return;
            }


            ProcessSkillCastInRadius(caster, position, aoeRadius.Value, skillDamage.Value, areaOfEffectMask);
        }

        private bool ProcessSpRequirement(CharacterAttributes caster)
        {
            if (caster.CurrentSp < SpCost())
            {
                Debug.Log("Not casting "+ this + " Low SP! Needs " + SpCost());
                return false;
            }

            caster.CurrentSp -= SpCost();
            return true;
        }
    }
}
