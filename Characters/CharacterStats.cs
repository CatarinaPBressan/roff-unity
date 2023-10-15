using UnityEngine;

namespace Characters
{
    public class CharacterStats: MonoBehaviour
    {
        [field: SerializeField] public int BaseStr { get; protected set; } = 1;
        [field: SerializeField] public int BaseVit { get; protected set; } = 1;
        [field: SerializeField] public int BaseInt { get; protected set; } = 1;
    }
}
