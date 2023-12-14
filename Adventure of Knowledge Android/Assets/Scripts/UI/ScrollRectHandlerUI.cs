using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdventureOfKnowledge.UI
{
    public class ScrollRectHandlerUI:ScrollRect
    {
        private const int LERP_VELOCITY_BOUNDRY = 150;

        private bool isNotDragging;
        private float spaceBetweenElement;
        private float lerpPosition;

        private readonly float scrollSpeed = 2f;
         
        protected override void Awake()
        {
            base.Awake();

            spaceBetweenElement = 1f / (content.childCount - 1);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            isNotDragging = false;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            isNotDragging = true;

            
        }
        private void Update()
        {
            
            if (!isNotDragging) return;
            
            if(Mathf.Abs(velocity.x) < LERP_VELOCITY_BOUNDRY)
            {
                float leftBound = (int)(normalizedPosition.x / spaceBetweenElement) * spaceBetweenElement;
                float rightBound = leftBound + spaceBetweenElement;

                lerpPosition = (normalizedPosition.x - leftBound <= rightBound - normalizedPosition.x) ? leftBound : rightBound;

                normalizedPosition = Vector2.Lerp(normalizedPosition, new Vector2(lerpPosition, normalizedPosition.y), Time.deltaTime * scrollSpeed);
            }
         
        }

    }
}
