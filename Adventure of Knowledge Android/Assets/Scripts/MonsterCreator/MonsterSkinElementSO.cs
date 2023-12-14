using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [CreateAssetMenu(fileName = "MonsterSkinElement", menuName = "ScriptableObject/MonsterSkinElement")]
    public class MonsterSkinElementSO:ScriptableObject
    {
        [SerializeField] private List<MonsterSkinElement> monsterSkinElementList;
        [field: SerializeField] public TypeOfBodyPart BodyPart { get;private set; }
        [field: SerializeField] public int LayerLevel { get; private set; }

        public Sprite GetMonsterSkinElemntColorVaraint(int skinElemntIndex, int colorVaraintIndex)
        {
            if (monsterSkinElementList[skinElemntIndex].colorVariants.Count == 0)
                return monsterSkinElementList[skinElemntIndex].skinElement;
            else
                return monsterSkinElementList[skinElemntIndex].colorVariants[colorVaraintIndex];
        }

        public int GetMonsterSkinElemntCount() => monsterSkinElementList.Count;

        public int GetColorVaraintsCount(int skinElemntIndex) => monsterSkinElementList[skinElemntIndex].colorVariants.Count;

        public int GetMonsterSkinElementPrice(int skinElementIndex) => monsterSkinElementList[skinElementIndex].price; 
 
    }
    [Serializable]
    public struct MonsterSkinElement
    {
        public int price;
        public Sprite skinElement;
        public List<Sprite> colorVariants;

    }

    [Serializable]
    public struct AvailableMonsterSkinElementSaveData
    {
        public int indexOnList;
        public TypeOfBodyPart typeOfBodyPart;

        public AvailableMonsterSkinElementSaveData(int indexOnList, TypeOfBodyPart typeOfBodyPart)
        {
            this.indexOnList = indexOnList;
            this.typeOfBodyPart = typeOfBodyPart;
        }
    }


}
