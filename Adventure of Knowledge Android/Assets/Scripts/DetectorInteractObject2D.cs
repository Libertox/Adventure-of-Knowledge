using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class DetectorInteractObject2D:MonoBehaviour
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

            RaycastHit2D raycastHit2D = Physics2D.Raycast(ray.origin, ray.direction, maxDistance, interactLayerMask);
            if (raycastHit2D)
            {
                if (raycastHit2D.transform.TryGetComponent(out IInteractable interactable))
                    interactable.Interact();

            }           

        }
    }
}
