using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.MemoryGame
{
    public class MemoryTile : MonoBehaviour, IInteractable
    {
        private const float ANIMATION_DURATION = 0.5f;

        [SerializeField] private MemoryTileVisual memoryTileVisual;

        public int Index { get; private set; }

        public void Interact()
        {
            if (GameManager.Instance.IsPause()) return;

            MemoryGameManager memoryGameManager = GameManager.Instance as MemoryGameManager;

            if (!memoryGameManager.IsBothTileSelected())
            {
                transform.DORotate(new Vector3(0, 180f, 0), ANIMATION_DURATION);
                SoundManager.Instance.PlayInteractSound();
                memoryGameManager.SelectMemoryTile(this);
            }
           
        }

        public void Cover() => transform.DORotate(Vector3.zero, ANIMATION_DURATION);

        public void Dissolve() => StartCoroutine(memoryTileVisual.DissolveCoroutine());

        public void Initialize(int index, Sprite icon)
        {
            Index = index;
            memoryTileVisual.SetMemoryTileIcon(icon);   
        }


    }
}
