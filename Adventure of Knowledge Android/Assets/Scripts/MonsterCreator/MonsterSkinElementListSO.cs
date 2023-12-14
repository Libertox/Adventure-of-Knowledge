using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [CreateAssetMenu(fileName = "MonsterSkinElementList", menuName = "ScriptableObject/MonsterSkinElementList")]
    public class MonsterSkinElementListSO:ScriptableObject
    {
        [SerializeField] private List<MonsterSkinElementSO> monsterSkinElements;

        public MonsterSkinElementSO GetMonsterSkinElementFromBodyPart(TypeOfBodyPart bodyPart)
        {
            foreach (var skinElement in monsterSkinElements)
            {
                if (skinElement.BodyPart == bodyPart)
                    return skinElement;
            }
            return default;
        }
    }
}
