using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class DetectorInteractObject3D:MonoBehaviour
    {
        [SerializeField] private LayerMask interactLayerMask;

        private void Start() 
        {
            GameInputManager.Instance.OnScreenTouched += GameInputManager_OnScreenTouched;
        }

        private void GameInputManager_OnScreenTouched(object sender, GameInputManager.OnScreenTouchedEventArgs e)
        {
            DetectInteractObject(e.touchPosition);
        }

        private void DetectInteractObject(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            float maxDistance = 20f;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, maxDistance, interactLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out IInteractable interactable))
                    interactable.Interact();              
            }    
        }
    }
}
