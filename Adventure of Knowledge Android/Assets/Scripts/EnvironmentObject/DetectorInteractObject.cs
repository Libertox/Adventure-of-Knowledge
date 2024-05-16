using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class DetectorInteractObject:MonoBehaviour
    {
        [SerializeField] private LayerMask interactLayerMask;
        [SerializeField] private TypeOfDetectObject typeOfDetectObject;

        [Flags]
        private enum TypeOfDetectObject
        {
            Object2D,
            Object3D
        }

        private void Start() 
        {
            GameInputManager.Instance.OnScreenTouched += GameInputManager_OnScreenTouched;
            GameInputManager.Instance.OnScreenTouchedCanceled += GameInputManager_OnScreenTouchedCanceled;
        }

        private void GameInputManager_OnScreenTouchedCanceled(object sender, GameInputManager.OnScreenTouchedEventArgs e)
        {
            Ray ray = Camera.main.ScreenPointToRay(e.touchPosition);
            float maxDistance = 20f;

            if (typeOfDetectObject.HasFlag(TypeOfDetectObject.Object2D))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, maxDistance, interactLayerMask);
                if (raycastHit2D)
                {
                    if (raycastHit2D.transform.TryGetComponent(out IDraggable draggable))
                        draggable.Drop();
                }
            }

            if (typeOfDetectObject.HasFlag(TypeOfDetectObject.Object3D))
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, interactLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out IDraggable draggable))
                        draggable.Drop();
                }
            }

            
        }

        private void GameInputManager_OnScreenTouched(object sender, GameInputManager.OnScreenTouchedEventArgs e)
        {
            DetectObject(e.touchPosition);
        }

        private void DetectObject(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            float maxDistance = 20f;

            if (typeOfDetectObject.HasFlag(TypeOfDetectObject.Object2D))
            {
                RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, maxDistance, interactLayerMask);
                if (raycastHit2D)
                {
                    if (raycastHit2D.transform.TryGetComponent(out IInteractable interactable))
                        interactable.Interact();
                    if (raycastHit2D.transform.TryGetComponent(out IDraggable draggable))
                        draggable.Drag();

                }
            }

            if (typeOfDetectObject.HasFlag(TypeOfDetectObject.Object3D))
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, interactLayerMask))
                {
                     if (raycastHit.transform.TryGetComponent(out IInteractable interactable))
                         interactable.Interact();
                     if(raycastHit.transform.TryGetComponent(out IDraggable draggable))
                         draggable.Drag();
                }
            }
                          
        }
    }
}
