using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [RequireComponent(typeof(MonsterBodyPart))]
    public class MonsterBodyPartController:MonoBehaviour
    {
        private Vector3 pivotDistance;
        private MonsterBodyPart monsterBodyPart;


        private void Awake() => monsterBodyPart = GetComponent<MonsterBodyPart>();
        
        private void OnMouseDown() => 
            pivotDistance = Camera.main.ScreenToWorldPoint(GameInputManager.Instance.GetControllerPosition()) - transform.position;
       
        private void OnMouseUp()
        {
            if (!MonsterCreatorManager.Instance.CanPutBodyPart(monsterBodyPart.transform.position, monsterBodyPart.GetSpriteSize(), out Transform parentTransform))
                monsterBodyPart.DestroySelf();

            monsterBodyPart.transform.parent = parentTransform;
            pivotDistance = Vector3.zero;
        }

        private void OnMouseDrag()
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(GameInputManager.Instance.GetControllerPosition()) - pivotDistance;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

    }
}
