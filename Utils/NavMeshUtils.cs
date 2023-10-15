using UnityEngine;
using UnityEngine.AI;

namespace Utils
{
    public static class NavMeshUtils
    {
        public static Vector3? GetRandomNavMeshPoint(Vector3 center, float surveyorRange)
        {
            for (var i = 0; i < 30; i++)
            {
                var randomPoint = center + Random.insideUnitSphere * surveyorRange;
                if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return null;
        }
    }
}
