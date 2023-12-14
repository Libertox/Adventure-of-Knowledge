using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class MonsterVisual:MonoBehaviour
    {
        [field: SerializeField] public MonsterBodyPart MonsterBody { get; private set; }

        [field: SerializeField] public MonsterSkinElementListSO MonsterSkinElementListSO { get;private set; }

        public Vector2 GetBodyPosition() => MonsterBody.transform.position;
    }
}
