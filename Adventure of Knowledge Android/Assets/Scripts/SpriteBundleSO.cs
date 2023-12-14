using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [CreateAssetMenu(fileName = "SpriteBundle", menuName = "ScriptableObject/SpriteBundle")]
    public class SpriteBundleSO:ScriptableObject
    {
        [SerializeField] private List<Sprite> spriteList;

        public Sprite GetRandomSprite()
        {
            int spriteListIndex = UnityEngine.Random.Range(0, spriteList.Count);
            return spriteList[spriteListIndex];
        }

    }
}
