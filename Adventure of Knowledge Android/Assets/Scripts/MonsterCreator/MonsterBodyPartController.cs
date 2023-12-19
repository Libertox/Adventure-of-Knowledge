using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [RequireComponent(typeof(MonsterBodyPart))]
    public class MonsterBodyPartController:MonoBehaviour,IDraggable
    {
        private Vector3 pivotDistance;
        private MonsterBodyPart monsterBodyPart;

        private DragState dragState;

        private void Awake() => monsterBodyPart = GetComponent<MonsterBodyPart>();

      
        public void Drag()
        {
            pivotDistance = Camera.main.ScreenToWorldPoint(GameInputManager.Instance.GetControllerPosition()) - transform.position;
            dragState = DragState.IsDrag;
        }

        public void Drop()
        {
            if (!MonsterCreatorManager.Instance.CanPutBodyPart(monsterBodyPart.transform.position, monsterBodyPart.GetSpriteSize(), out Transform parentTransform))
                monsterBodyPart.DestroySelf();

            monsterBodyPart.transform.parent = parentTransform;
            pivotDistance = Vector3.zero;

            dragState = DragState.IsDrop;
        }

        private void Update()
        {
            if (dragState != DragState.IsDrag) return;


            Vector3 newPosition = Camera.main.ScreenToWorldPoint(GameInputManager.Instance.GetControllerPosition()) - pivotDistance;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }

    }
}
