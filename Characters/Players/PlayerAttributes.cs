using System;
using Characters.Monsters;
using Characters.Players.PlayerClasses;
using Characters.Skills;
using Enums;
using UnityEngine;
using Utils;

namespace Characters.Players
{
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerAttributes: CharacterAttributes
    {
        private int _maxHp;
        private int _maxSp;
        private int _baseExp;
        private int _jobExp;
        private int _baseLevel = 1;
        private int _jobLevel = 1;
        private int _baseLevelThreshold;
        private int _jobLevelThreshold;

        private ActionTimer _hpRegenTimer;
        private ActionTimer _spRegenTimer;

        private PlayerController _playerController;
        [SerializeField] private PlayerStats playerStats;

        public bool IsDead { get; private set; }
        private PlayerClass _playerClass = new AdventurerClass();

        protected override void Start()
        {
            base.Start();

            _playerController = GetComponent<PlayerController>();

            _baseLevelThreshold = GetBaseLevelExpThreshold(_baseLevel);
            _jobLevelThreshold = GetJobLevelExpThreshold(_jobLevel);

            UpdateMaxHp();
            UpdateMaxSp();

            CurrentHp = _maxHp;
            CurrentSp = _maxSp;

            _hpRegenTimer = new ActionTimer(8f, RegenHp);
            _spRegenTimer = new ActionTimer(4f, RegenSp);
        }

        private void Update()
        {
            if (IsDead)
            {
                return;
            }
            _hpRegenTimer.Elapse(Time.deltaTime);
            _spRegenTimer.Elapse(Time.deltaTime);
        }

        private int GetBaseLevelExpThreshold(int currentBaseLevel)
        {
            return currentBaseLevel * 100;
        }

        private void RegenSp()
        {
            CurrentSp = Math.Clamp(CurrentSp + SpRegenTick(), 0, _maxSp);
        }

        private void RegenHp()
        {
            CurrentHp = Math.Clamp(CurrentHp + HpRegenTick(), 0, _maxHp);
        }

        private int HpRegenTick()
        {
            var hpRegenTick = (int)(
                10 +
                _baseLevel * 3 +
                Math.Ceiling(playerStats.BaseStr / 25f) +
                Math.Ceiling(playerStats.BaseVit / 10f)
            );
            return hpRegenTick;
        }

        private void UpdateMaxSp()
        {
            _maxSp = (int)Math.Floor(
                (
                    50 +
                    _baseLevel * 10 +
                    (int)Math.Floor(playerStats.BaseInt / 8f)
                ) *
                _playerClass.MaxSpFactor
            );
            CurrentSp = Math.Clamp(CurrentSp, 1, _maxSp);
        }

        private void UpdateMaxHp()
        {
            _maxHp = (int)Math.Floor(
                (
                    150 +
                    _baseLevel * 5 +
                    (int)Math.Floor(playerStats.BaseVit / 3f) +
                    (int)Math.Floor(playerStats.BaseStr / 10f)
                ) *
                _playerClass.MaxHpFactor
            );
            CurrentHp = Math.Clamp(CurrentHp, 1, _maxHp);
        }

        private void OnGUI()
        {
            var baseLevelPct = (int)((float)_baseExp / GetBaseLevelExpThreshold(_baseLevel) * 100);
            var jobLevelPct = (int)((float)_jobExp / GetJobLevelExpThreshold(_jobLevel) * 100);
            GUI.Box(new Rect(0,120, 200, 240), "Character attributes");
            GUI.Label(new Rect(10, 150, 180,20), "Base Lvl: " + _baseLevel + " " +
                                                 baseLevelPct + "% (" + _baseExp + " / " + GetBaseLevelExpThreshold(_baseLevel) + ")");
            GUI.Label(new Rect(10, 170, 180,20), "Job Lvl: " + _jobLevel + " " +
                                                 jobLevelPct + "% (" + _jobExp + " / " + GetJobLevelExpThreshold(_jobLevel) + ")");
            GUI.Label(new Rect(10, 210, 180, 20), "HP: " + CurrentHp + " / " + _maxHp + " (" + HpRegenTick() + ")");
            GUI.Label(new Rect(10, 230, 180, 20), "SP: " + CurrentSp + " / " + _maxSp + " (" + SpRegenTick() + ")");

            GUI.Label(new Rect(10, 250, 180, 20), _playerClass.Name + " Skills: ");
            const int w = 25;
            for (var i = 0; i < _playerClass.Skills.Length; i++)
            {
                var skill = _playerClass.Skills[i];
                var normI = i + 1;
                GUI.Label(new Rect(10, 270, 180, 20), skill.Name);
                if (GUI.Button(new Rect(10 + w * normI, 290, w, 20), normI.ToString()))
                {
                    _playerController.CueSkill(skill);
                }
            }

            if (GUI.Button(new Rect(10, 310, 90, 20), "Change to Adventurer"))
            {
                ClassUpdated(new AdventurerClass());
            }
            if (GUI.Button(new Rect(100, 310, 90, 20), "Change to Soldier"))
            {
                ClassUpdated(new SoldierClass());
            }
            if (GUI.Button(new Rect(190, 310, 90, 20), "Change to Mage"))
            {
                ClassUpdated(new MageClass());
            }

            if (IsDead && GUI.Button(new Rect(10, 350, 180, 20), "Resurrect player"))
            {
                transform.position = Vector3.zero;
                IsDead = false;
                CurrentHp = (int) Math.Ceiling(_maxHp * 0.5f);
            }
        }

        private int GetJobLevelExpThreshold(int currentJobLevel)
        {
            return 150 * currentJobLevel;
        }

        private int SpRegenTick()
        {
            var spRegenTick = (int)(
                15 +
                _baseLevel * 4 +
                Math.Ceiling(playerStats.BaseInt / 8f)
            );
            return spRegenTick;
        }

        private void ClassUpdated(PlayerClass playerClass)
        {
            _playerClass = playerClass;
            UpdateMaxHp();
            UpdateMaxSp();
        }

        public void StatUpdated(Stat stat)
        {
            switch (stat)
            {
                case Stat.Int:
                    UpdateMaxSp();
                    break;
                case Stat.Str:
                    UpdateMaxHp();
                    break;
                case Stat.Vit:
                    UpdateMaxHp();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stat), stat, "Unknown stat");
            }
        }

        public void AddBaseExp(int amount)
        {
            _baseExp += amount;

            if (_baseExp >= _baseLevelThreshold)
            {
                _baseLevel++;
                _baseExp -= _baseLevelThreshold;

                _baseLevelThreshold = GetBaseLevelExpThreshold(_baseLevel);
            }
        }

        public void AddJobExp(int amount)
        {
            _jobExp += amount;

            if (_jobExp >= _jobLevelThreshold)
            {
                _jobLevel++;
                _jobExp -= _jobLevelThreshold;

                _jobLevelThreshold = GetJobLevelExpThreshold(_jobLevel);
            }
        }

        public override int GetCharacterPhysicalDamage()
        {
            //https://www.geogebra.org/calculator/peqwpckz

            var baseStrFactor = (int)Math.Floor(playerStats.BaseStr / 10f);
            return (int)(playerStats.BaseStr + Math.Pow(baseStrFactor, 2));
        }

        public override int GetCharacterMagicalDamage()
        {
            //https://www.geogebra.org/calculator/wqazcpdx

            var baseIntFactor = (int)Math.Floor(playerStats.BaseInt / 3f);
            return (int)(playerStats.BaseStr + Math.Pow(baseIntFactor, 2));
        }

        protected override void OnDeath(CharacterAttributes attackerAttributes)
        {
            IsDead = true;
            if (attackerAttributes.TryGetComponent<MonsterController>(out var monster))
            {
               monster.ClearTarget();
            }

            var onePercent = (int) Math.Floor(_baseLevelThreshold / 100f);
            _baseExp = Math.Clamp(_baseExp - onePercent, 0, _baseLevelThreshold);
        }
    }
}
