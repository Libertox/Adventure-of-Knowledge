using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class MonsterVisualCreator:MonsterVisual
    {  
        private void Start()
        {
            MonsterCreatorManager.Instance.OnBodyChanged += MonsterCreatorManager_OnBodyChanged;

            List<BodyPartSaveData> bodyPartSaveDatas = SaveManager.LoadMonsterVisual();

            if (bodyPartSaveDatas == null) return;

            DeleteBasicMonsterVisual();

            foreach (var item in bodyPartSaveDatas)
                CreateLoadedBodyPart(item, transform);
        }

        private void CreateLoadedBodyPart(BodyPartSaveData bodyPartSaveData, Transform parent)
        {
            if (bodyPartSaveData.IsBody())
            {
                UpdateMonsterBody(bodyPartSaveData.spriteIndex, bodyPartSaveData.colorIndex);
                return;
            }

            MonsterBodyPart monsterBodyPart = MonsterCreatorManager.Instance.InstantiateMonsterBodyPart(bodyPartSaveData, parent);

            foreach (var child in bodyPartSaveData.child)
                CreateLoadedBodyPart(child, monsterBodyPart.transform);
        }

        private void DeleteBasicMonsterVisual()
        {
            foreach(Transform elementTransform in transform)
            {
                if (elementTransform.TryGetComponent(out MonsterBodyPart monsterBodyPart))
                {
                    if(!monsterBodyPart.IsBodyElement())
                        monsterBodyPart.DestroySelf();
                }     
            }
        }

        private void MonsterCreatorManager_OnBodyChanged(object sender, MonsterCreatorManager.OnBodyChangedEventArgs e) => 
            UpdateMonsterBody(e.skinInex, e.colorIndex);
       
        private void UpdateMonsterBody(int skinIndex, int colorIndex)
        {
            MonsterSkinElementSO monsterSkinElementsSO = MonsterSkinElementListSO.GetMonsterSkinElementFromBodyPart(TypeOfBodyPart.Body);
            MonsterBody.SetSprite(monsterSkinElementsSO.GetMonsterSkinElemntColorVaraint(skinIndex, colorIndex));
            MonsterBody.SetId(skinIndex);
            MonsterBody.SetColorIndex(colorIndex);
            PolygonCollider2D polygonCollider2D = MonsterBody.gameObject.GetComponent<PolygonCollider2D>();
            Destroy(polygonCollider2D);
            MonsterBody.gameObject.AddComponent<PolygonCollider2D>();

            DestroyNotFitBodyPart(transform);
        }

        private void DestroyNotFitBodyPart(Transform parent)
        {
            foreach(Transform child in parent)
            {
                DestroyNotFitBodyPart(child);

                MonsterBodyPart monsterBodyPart = child.GetComponent<MonsterBodyPart>();

                if (monsterBodyPart.IsBodyElement()) continue;

                if (!MonsterCreatorManager.Instance.CanPutBodyPart(monsterBodyPart.transform.position, monsterBodyPart.GetSpriteSize(), out Transform parentTransform))
                    monsterBodyPart.DestroySelf();    
            }
        }
    }
}
