using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.MemoryGame
{
    public class MemoryTileVisual:MonoBehaviour
    {
        private const string SHADER_MAIN_TEXTURE_REF = "_MainTexture";
        private const string SHADER_DISSOLVE_AMOUNT_REF = "_DissolveAmount";

        [SerializeField] private SpriteRenderer spriteRenderer;

        private Material modelMaterial;

        private void Start() 
        {
            modelMaterial = GetComponent<MeshRenderer>().material;
            StartCoroutine(AppearCoroutine());
        }

     
        public void SetMemoryTileIcon(Sprite tileIcon) 
        {
            spriteRenderer.sprite = tileIcon;
            spriteRenderer.material.SetTexture(SHADER_MAIN_TEXTURE_REF, tileIcon.texture);
        } 

        public IEnumerator DissolveCoroutine()
        {
            float dissoloveAmount = 1;
            while (dissoloveAmount > 0)
            {
                dissoloveAmount -= Time.deltaTime;
                modelMaterial.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, dissoloveAmount);
                spriteRenderer.material.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, dissoloveAmount);
                yield return null;
            }
            Destroy(transform.parent.gameObject);       
        }

        public IEnumerator AppearCoroutine()
        {
            float appearAmount = 0;
            while(appearAmount < 1)
            {
                appearAmount += Time.deltaTime;
                modelMaterial.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, appearAmount);
                spriteRenderer.material.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, appearAmount);

                yield return null;
            }
        }
    }
}
