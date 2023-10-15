using System;
using Characters.Skills;

namespace Characters.Players.PlayerClasses
{
    public class AdventurerClass : PlayerClass
    {
        public AdventurerClass() : base(
            "Adventurer",
            0.5f,
            0.25f,
            Array.Empty<Skill>())
        {
        }
    }
}
