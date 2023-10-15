using Characters.Skills;

namespace Characters.Players.PlayerClasses
{
    public class MageClass : PlayerClass
    {
        public MageClass() : base(
            "Mage",
            0.6f,
            0.75f,
            new Skill[]
            {
                new Fireball(1),
                new Fireball(10)
            }) {}
    }
}
