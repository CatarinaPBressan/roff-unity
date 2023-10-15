using Enums;
using UnityEngine;

namespace Characters.Players
{
    public class PlayerStats : CharacterStats
    {
        private PlayerAttributes _playerAttributes;

        private void Start()
        {
            _playerAttributes = gameObject.GetComponent<PlayerAttributes>();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0,0, 150, 120), "Character stats");
            //STR
            GUI.Label(new Rect(10, 30, 50, 20), "STR");
            GUI.Label(new Rect(60, 30, 30, 20), BaseStr.ToString());
            if (GUI.RepeatButton(new Rect(80, 30, 20, 20), "-"))
            {
                BaseStr = BaseStr <= 1 ? 1 : BaseStr - 1;
                _playerAttributes.StatUpdated(Stat.Str);
            }
            if (GUI.RepeatButton(new Rect(100, 30, 20, 20), "+"))
            {
                BaseStr = BaseStr >= 99 ? 99 : BaseStr + 1;
                _playerAttributes.StatUpdated(Stat.Str);
            }
            //INT
            GUI.Label(new Rect(10, 60, 50, 20), "INT");
            GUI.Label(new Rect(60, 60, 30, 20), BaseInt.ToString());
            if (GUI.RepeatButton(new Rect(80, 60, 20, 20), "-"))
            {
                BaseInt = BaseInt <= 1 ? 1 : BaseInt - 1;
                _playerAttributes.StatUpdated(Stat.Int);
            }
            if (GUI.RepeatButton(new Rect(100, 60, 20, 20), "+"))
            {
                BaseInt = BaseInt >= 99 ? 99 : BaseInt + 1;
                _playerAttributes.StatUpdated(Stat.Int);
            }

            //VIT
            GUI.Label(new Rect(10, 90, 50, 20), "VIT");
            GUI.Label(new Rect(60, 90, 30, 20), BaseVit.ToString());
            if (GUI.RepeatButton(new Rect(80, 90, 20, 20), "-"))
            {
                BaseVit = BaseVit <= 1 ? 1 : BaseVit - 1;
                _playerAttributes.StatUpdated(Stat.Vit);
            }
            if (GUI.RepeatButton(new Rect(100, 90, 20, 20), "+"))
            {
                BaseVit = BaseVit >= 99 ? 99 : BaseVit + 1;
                _playerAttributes.StatUpdated(Stat.Vit);
            }

        }
    }
}
