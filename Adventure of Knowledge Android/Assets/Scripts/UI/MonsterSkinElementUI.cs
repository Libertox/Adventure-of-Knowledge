using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class MonsterSkinElementUI:MonoBehaviour
    {
        [SerializeField] private Button skinElementButton;

        [SerializeField] private Image skinElementImage;
        [SerializeField] private PaddlockElementUI paddlock;

        private int selectedIndex;

        private void Awake()
        {
            skinElementButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                MonsterCreatorManager.Instance.SelectSkinElement(selectedIndex);             
            });         
        }

        private void Start()
        {
            MonsterCreatorManager.Instance.BuyingManager.OnNewSkinElementBought += MonsterCreatorManager_OnNewSkinElementBought;
        }

        private void MonsterCreatorManager_OnNewSkinElementBought(object sender, EventArgs e)
        {
            if (MonsterCreatorManager.Instance.SelectedSkinElement == selectedIndex)
            {
                StartCoroutine(paddlock.DissolveCoroutine());
            }
                
        }

        public void UpdateVisual(MonsterSkinElementSO monsterSkinElementSO, int selectedIndex)
        {
            this.selectedIndex = selectedIndex;
            skinElementImage.sprite = monsterSkinElementSO.GetMonsterSkinElemntColorVaraint(selectedIndex, MonsterCreatorManager.Instance.SelectedColor); 
        }

        public void SetAvailableSkinElement(bool isAvailable)
        {
            paddlock.SetActive(!isAvailable);
        }

        private void OnDestroy()
        {
            MonsterCreatorManager.Instance.BuyingManager.OnNewSkinElementBought -= MonsterCreatorManager_OnNewSkinElementBought;
        }
    }
}
