using System;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class CloudObject:MonoBehaviour
    {
        [SerializeField] private Transform background;
        [SerializeField] private SpriteBundleSO spriteBundle;

        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;

        private float rightBoundOfBackground;
        private float leftBoundOfBackground;
        private float upBoundOfBackground;
        private float downBoundOfBackground;

        private float speed;

        private SpriteRenderer spriteRenderer;
        
        private void Awake()
        {
            speed = GetRandomSpeed();
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetBackgroundBound();
            SetRandomTransparency();
        }

        private void SetBackgroundBound()
        {
            Sprite sprite = background.GetComponent<SpriteRenderer>().sprite;

            float spirteBoundX = sprite.bounds.size.x;
            float spriteBoundY = sprite.bounds.size.y;
            float backgroundXSize = background.localScale.x;
            float backgroundYSize = background.localScale.y;

            rightBoundOfBackground = background.position.x + (spirteBoundX * backgroundXSize * 0.5f);
            leftBoundOfBackground = background.position.x - (spirteBoundX * backgroundXSize * 0.5f);

            upBoundOfBackground = background.position.y + (spriteBoundY * backgroundYSize * 0.5f);
            downBoundOfBackground = background.position.y - (spriteBoundY * backgroundYSize * 0.5f);

        }

        private float GetRandomSpeed() => UnityEngine.Random.Range(minSpeed, maxSpeed);

        private void SetRandomTransparency()
        {
            float minTransparency = 0.2f;
            float transparency = UnityEngine.Random.Range(minTransparency, 1);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, transparency);
        }

        private void Update()
        {
            transform.Translate(Vector2.right * (speed * Time.deltaTime));

            if (IsCrossRightBorder())
            {
                SetSpriteAndSpeed();
                SetRandomTransparency();
                transform.position = new Vector3(leftBoundOfBackground + .5f, GetRandomPositionYOnBackground(), transform.position.z);
            }
            if (IsCrossLeftBorder())
            {
                SetSpriteAndSpeed();
                SetRandomTransparency();
                transform.position = new Vector3(rightBoundOfBackground - .5f, GetRandomPositionYOnBackground(), transform.position.z);
            }
                
        }

        private bool IsCrossRightBorder() => transform.position.x > rightBoundOfBackground;

        private bool IsCrossLeftBorder() => transform.position.x < leftBoundOfBackground;

        private void SetSpriteAndSpeed()
        {
            speed = GetRandomSpeed();
            spriteRenderer.sprite = spriteBundle.GetRandomSprite();
        }

        private float GetRandomPositionYOnBackground() => UnityEngine.Random.Range(downBoundOfBackground, upBoundOfBackground);
    }
}
