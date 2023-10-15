using Characters.Skills;

namespace Characters.Players.PlayerClasses
{
    public class SoldierClass: PlayerClass
    {
        public SoldierClass() : base(
            "Soldier",
            0.75f,
            0.3f,
            new Skill[]
            {
                new Bash(1),
                new Bash(10)
            }) {}
    }
}
