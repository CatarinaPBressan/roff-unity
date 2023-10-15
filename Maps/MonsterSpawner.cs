using System.Collections.Generic;
using Characters.Monsters;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Maps
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject monster;
        [Range(0, 18000)] [SerializeField] private int spawnRate;
        [Range(1, 100)] [SerializeField] private int maxSpawnCount = 10;
        [Range(0f, 30f)] [SerializeField] private float spawnRange = 10f;

        private Bounds _spawnBounds;
        private ActionTimer _spawnTimer;
        private List<GameObject> _monsters;

        private void Start()
        {
            _spawnBounds = GetComponentInChildren<Collider>().bounds;
            _spawnTimer = new ActionTimer(spawnRate, SpawnMonsters);
            _monsters = new List<GameObject>(maxSpawnCount);
            SpawnMonsters();
        }

        private void Update()
        {
            _spawnTimer.Elapse(Time.deltaTime);
        }

        private void SpawnMonsters()
        {
            var currentCount = _monsters.Count;
            if (currentCount == maxSpawnCount)
            {
                return;
            }

            var countToAdd = maxSpawnCount - currentCount;
            for (var i = 0; i < countToAdd; i++)
            {
                var randomInBoundsPoint = new Vector3(
                    Random.Range(_spawnBounds.min.x, _spawnBounds.max.x),
                    Random.Range(_spawnBounds.min.y, _spawnBounds.max.y),
                    Random.Range(_spawnBounds.min.z, _spawnBounds.max.z)
                );
                var foundPoint = NavMeshUtils.GetRandomNavMeshPoint(randomInBoundsPoint, spawnRange);
                if (!foundPoint.HasValue)
                {
                    continue;
                }

                var spawnedMonster = Instantiate(monster, foundPoint.Value, Quaternion.identity);
                _monsters.Add(spawnedMonster);

                if (spawnedMonster.TryGetComponent<MonsterController>(out var monsterController))
                {
                    monsterController.MonsterDeath += HandleMonsterDeath;
                }
            }
        }

        private void HandleMonsterDeath(object sender, MonsterDeathEventArgs args)
        {
            if (!_monsters.Contains(args.MonsterGameObject))
            {
                return;
            }

            _monsters.Remove(args.MonsterGameObject);
            if (args.MonsterGameObject.TryGetComponent<MonsterController>(out var monsterController))
            {
                monsterController.MonsterDeath -= HandleMonsterDeath;
            }
        }
    }
}
