using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class TextTileVisual:MonoBehaviour
    {
        private const string SHADER_DISSOLVE_AMOUNT_REF = "_DissolveAmount";

        public event EventHandler OnDisappeared;

        [SerializeField] private TextMeshPro dignitText;

        private SpriteRenderer spriteRenderer;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            Material material = new Material(spriteRenderer.sharedMaterial);
            spriteRenderer.sharedMaterial = material;
        }

        private void Start()
        {
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
            GameManager.Instance.OnCorrectAnswer += GameManager_OnCorrectAnswer;
            GameManager.Instance.OnNewStageLoaded += GameManager_OnNewStageLoaded;

        }

        private void GameManager_OnNewStageLoaded(object sender, EventArgs e) 
        {
            if(gameObject.activeInHierarchy)
                StartCoroutine(AppearCoroutine());
        } 

        private void GameManager_OnCorrectAnswer(object sender, EventArgs e) 
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(DisappearCoroutine());
        } 
       
        private void GameManager_OnGameStarted(object sender, GameManager.OnGameStartedEventArgs e) 
            => StartCoroutine(AppearCoroutine());
       
        public IEnumerator AppearCoroutine()
        {
            float dissoloveAmount = 0;
            while (dissoloveAmount < 1)
            {
                dissoloveAmount += Time.deltaTime;
                spriteRenderer.sharedMaterial.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, dissoloveAmount);
                dignitText.color = new Color(dignitText.color.r, dignitText.color.g, dignitText.color.b, dissoloveAmount);
                yield return null;
            } 
        }

        private IEnumerator DisappearCoroutine()
        {
            float dissoloveAmount = 1;
            while (dissoloveAmount > 0)
            {
                dissoloveAmount -= Time.deltaTime;
                spriteRenderer.sharedMaterial.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, dissoloveAmount);
                dignitText.color = new Color(dignitText.color.r, dignitText.color.g, dignitText.color.b, dissoloveAmount);
                yield return null;
            }

            OnDisappeared?.Invoke(this, EventArgs.Empty);
        }

        public void UppdateText(string text) => dignitText.SetText(text);

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStarted -= GameManager_OnGameStarted;
            GameManager.Instance.OnCorrectAnswer -= GameManager_OnCorrectAnswer;
            GameManager.Instance.OnNewStageLoaded -= GameManager_OnNewStageLoaded;
        }

    }
}
