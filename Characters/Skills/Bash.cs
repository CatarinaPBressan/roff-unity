using System;

namespace Characters.Skills
{
    public class Bash : Skill
    {
        public Bash(int skillLevel) : base(skillLevel)
        {
            Name = "Bash";
        }

        public override float? Range()
        {
            return null;
        }

        protected override int? Damage(CharacterAttributes characterAttributes)
        {
            return (int) Math.Ceiling(characterAttributes.GetCharacterPhysicalDamage() * 1.5 * SkillLevel);
        }

        protected override int SpCost()
        {
            return SkillLevel switch
            {
                <= 3 => 8,
                > 3 and <= 7 => 13,
                _ => 15
            };
        }
    }
}
