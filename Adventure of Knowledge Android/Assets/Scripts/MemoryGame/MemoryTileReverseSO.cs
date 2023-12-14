using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.MemoryGame
{
    [CreateAssetMenu(fileName = "MemoryTileReverse", menuName = "ScriptableObject/MemoryTileReverse")]
    public class MemoryTileReverseSO:ScriptableObject
    {
        [field: SerializeField] public List<Sprite> MemoryTileReverseList { get; private set; }


    }
}
