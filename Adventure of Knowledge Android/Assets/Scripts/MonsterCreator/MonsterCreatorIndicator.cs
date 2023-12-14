using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MonsterCreatorIndicator:MonoBehaviour
    {
        private SpriteRenderer indicatorSpriteRenderer;
        private bool isPress;

        private void Awake() => indicatorSpriteRenderer = GetComponent<SpriteRenderer>();
      
        private void Start()
        {
            GameInputManager.Instance.OnScreenTouched += GameInputManager_OnScreenTouched;
            GameInputManager.Instance.OnScreenTouchedCanceled += GameInputManager_OnScreenTouchedCanceled;

            MonsterCreatorManager.Instance.OnNewSkinElementChoosed += MonsterCreatorManager_OnNewSkinElementChoosed;

            Hide();
        }

        private void MonsterCreatorManager_OnNewSkinElementChoosed(object sender, MonsterCreatorManager.OnNewSkinElementChoosedEventArgs e)
        {
            UpdateVisual(e.newSkinElementSprite);
        }

        private void GameInputManager_OnScreenTouchedCanceled(object sender, EventArgs e)
        {
            if (!isPress || !gameObject.activeInHierarchy) return;

            isPress = false;

            Vector2 size = new Vector2(indicatorSpriteRenderer.sprite.bounds.size.x, indicatorSpriteRenderer.sprite.bounds.size.y);

            MonsterCreatorManager.Instance.SpawnMonsterBodyPart(transform.position, size);

            Hide();
        }

        private void GameInputManager_OnScreenTouched(object sender, GameInputManager.OnScreenTouchedEventArgs e) =>
            isPress = true;

        private void Update()
        {
            if (!isPress) return;

            Vector2 touchposition = Camera.main.ScreenToWorldPoint(GameInputManager.Instance.GetControllerPosition());
            transform.position = touchposition;
        }

        public void UpdateVisual(Sprite sprite)
        {
            indicatorSpriteRenderer.sprite = sprite;
            transform.position = GameInputManager.Instance.GetControllerPosition();
            Show();
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }

}
