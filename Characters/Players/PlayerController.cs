using System;
using Characters.Monsters;
using Characters.Skills;
using Enums;
using UnityEngine;

namespace Characters.Players
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerAttributes))]
    [RequireComponent(typeof(TargetPather))]
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private TargetPather targetPather;

        private PlayerAttributes _playerAttributes;
        private PlayerStats _playerStats;
        private Skill _cuedSkill;
        public Camera PlayerCamera { get; set; }

        private void Start()
        {
            _playerAttributes = GetComponent<PlayerAttributes>();
            _playerStats = GetComponent<PlayerStats>();

            targetPather.OnTargetReach = InteractWithTarget;
            targetPather.OnPositionReach = InteractWithPosition;
        }

        private void Update()
        {
            if (_playerAttributes.IsDead)
            {
                targetPather.ClearTarget();
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                targetPather.ClearTarget();
                var ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var raycastHit))
                {
                    if (_cuedSkill == null)
                    {
                        if (raycastHit.collider.gameObject.TryGetComponent<NonPlayerCharacter>(out var npc))
                        {
                            targetPather.SetTarget(npc.gameObject);
                        }
                        else
                        {
                            targetPather.SetDestination(raycastHit.point);
                        }
                    }
                    else
                    {
                        switch (_cuedSkill.SkillTargetType)
                        {
                            case SkillTargetType.Ground:
                            {
                                var skillRange = _cuedSkill.Range();
                                if (skillRange.HasValue)
                                {
                                    targetPather.SetDestination(raycastHit.point, skillRange.Value);
                                }
                                else
                                {
                                    targetPather.SetDestination(raycastHit.point);
                                }

                                break;
                            }
                            case SkillTargetType.SingleTarget:
                            {
                                if (raycastHit.collider.gameObject.TryGetComponent<MonsterController>(out var monsterController))
                                {
                                    targetPather.SetTarget(monsterController.gameObject, _cuedSkill.Range());
                                }

                                break;
                            }
                            case SkillTargetType.Self:
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1) && _cuedSkill != null)
            {
                Debug.Log("Un-queueing skill " + _cuedSkill);
                _cuedSkill = null;
            }
        }

        private void InteractWithTarget(GameObject target)
        {
            if (_playerAttributes.IsDead)
            {
                _cuedSkill = null;
                return;
            }

            if (target.TryGetComponent<MonsterAttributes>(out var monsterAttributes))
            {
                if (_cuedSkill != null)
                {
                    _cuedSkill.CastSkill(_playerAttributes, monsterAttributes, PhysicsLayers.Monster);
                }
                else
                {
                    monsterAttributes.BeDamagedByAttacker(_playerAttributes);
                }
            }

            _cuedSkill = null;
        }

        private void InteractWithPosition(Vector3 position)
        {
            if (_playerAttributes.IsDead)
            {
                _cuedSkill = null;
                return;
            }

            if (_cuedSkill is { SkillTargetType: SkillTargetType.Ground })
            {
                _cuedSkill.CastSkill(_playerAttributes, position, PhysicsLayers.Monster);
            }

            _cuedSkill = null;

        }

        public void CueSkill(Skill skill)
        {
            _cuedSkill = skill;
            Debug.Log("Queued " + skill + "for player");
        }
    }
}
