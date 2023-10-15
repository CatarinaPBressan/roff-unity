using Characters.Skills;

namespace Characters.Players.PlayerClasses
{
    public class PlayerClass
    {
        public string Name { get; private set; }
        public float MaxHpFactor { get; private set; }
        public float MaxSpFactor { get; private set; }

        public Skill[] Skills { get; private set; }

        protected PlayerClass(string name, float maxHpFactor, float maxSpFactor, Skill[] skills)
        {
            Name = name;
            MaxHpFactor = maxHpFactor;
            MaxSpFactor = maxSpFactor;
            Skills = skills;
        }
    }
}
