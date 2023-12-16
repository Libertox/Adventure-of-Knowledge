using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class MonsterVisualGame:MonsterVisual
    {
        [SerializeField] private MonsterBodyPart rightArm;

        [SerializeField] private bool hasLowerBody;

        private bool hasRightArm;

        private void Start()
        {
            List<BodyPartSaveData> bodyPartSaveDatas = SaveManager.LoadMonsterVisual();

            if (bodyPartSaveDatas == null) return;

            DeleteBasicMonsterVisual();

            foreach (var item in bodyPartSaveDatas)
            {
                if (CheckMonsterHasLeg(item)) continue;

                CreateLoadedBodyPart(item, transform);
            }

            if (!hasRightArm)
                Destroy(rightArm.gameObject);
        }

        private void CreateLoadedBodyPart(BodyPartSaveData bodyPartSaveData, Transform parent)
        {
            if (bodyPartSaveData.IsBody())
            {
                SetupBody(bodyPartSaveData);
                return;
            }

            if (bodyPartSaveData.IsRightArm())
            {
                SetupRightArm(bodyPartSaveData);
                hasRightArm = true;
                return;
            }
            
            
            if (CheckMonsterHasLeg(bodyPartSaveData)) return;

            MonsterBodyPart monsterBodyPart = Instantiate(MonsterBody);
            monsterBodyPart.Initialize(bodyPartSaveData, this, parent);

            foreach (var child in bodyPartSaveData.child)
                  CreateLoadedBodyPart(child, monsterBodyPart.transform);
                
        }

        private void SetupBody(BodyPartSaveData bodyPartSaveData)
        {
            MonsterSkinElementSO monsterSkinElementsSO = MonsterSkinElementListSO.GetMonsterSkinElementFromBodyPart(bodyPartSaveData.bodyPart);

            MonsterBody.SetSprite(monsterSkinElementsSO.GetMonsterSkinElemntColorVaraint(bodyPartSaveData.spriteIndex, bodyPartSaveData.colorIndex));
        }

        private void SetupRightArm(BodyPartSaveData bodyPartSaveData)
        {
            MonsterSkinElementSO monsterSkinElementsSO = MonsterSkinElementListSO.GetMonsterSkinElementFromBodyPart(bodyPartSaveData.bodyPart);

            rightArm.SetSprite(monsterSkinElementsSO.GetMonsterSkinElemntColorVaraint(bodyPartSaveData.spriteIndex, bodyPartSaveData.colorIndex));               
        }

        private bool CheckMonsterHasLeg(BodyPartSaveData bodyPartSaveData) => bodyPartSaveData.bodyPart == TypeOfBodyPart.Leg && !hasLowerBody;

        private void DeleteBasicMonsterVisual()
        {
            foreach (Transform elementTransform in transform)
            {
                if (elementTransform.TryGetComponent(out MonsterBodyPart monsterBodyPart))
                {
                    if (!monsterBodyPart.IsBodyElement() && !monsterBodyPart.IsRightArmElement())
                        Destroy(monsterBodyPart.gameObject);
                }
            }
        }

    }
}
