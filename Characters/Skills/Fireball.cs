using System;
using Enums;

namespace Characters.Skills
{
    public class Fireball : Skill
    {
        public Fireball(int skillLevel) : base(skillLevel)
        {
            SkillTargetType = SkillTargetType.Ground;
            Name = "Fireball";
        }

        public override float? Range()
        {
            return 10f;
        }

        protected override float? AoERadius()
        {
            return 4f;
        }

        protected override int? Damage(CharacterAttributes characterAttributes)
        {
            return (int) Math.Ceiling(characterAttributes.GetCharacterMagicalDamage() * 1.115 * SkillLevel);
        }

        protected override int SpCost()
        {
            //https://www.geogebra.org/calculator/x2nhcg2b
            return (int)Math.Ceiling(SkillLevel * 3.75);
        }
    }
}
