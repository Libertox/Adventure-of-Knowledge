using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.UI
{
    public class TextUIAnimation:MonoBehaviour
    {
        [SerializeField] private float scaleValue = 1.2f;
        [SerializeField] private float animationDuration = .5f;

        public void ChangeSizeText() 
        {
            Vector3 scaleVector = new Vector3 (scaleValue, scaleValue, 0);
            transform.DOScale(scaleVector, animationDuration).OnComplete(() => transform.DOScale(Vector3.one, animationDuration));
        }
          
    }
}
