using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class BuyingSkinElementUI:MonoBehaviour
    {
        [SerializeField] private Button buyMonsterSkinElementButton;

        [SerializeField] private TextMeshProUGUI requiredDiamondAmountText;

        [SerializeField] private TextUIAnimation requiredDiamondAmountTextAnimation;

        private void Awake()
        {
            buyMonsterSkinElementButton.onClick.AddListener(() =>
            {
                if (MonsterCreatorManager.Instance.BuyingManager.BuySkinElement())
                    Hide();
                else
                    requiredDiamondAmountTextAnimation.ChangeSizeText();
            });
        }

        private void Start()
        {

            MonsterBodyPart.OnSelected += MonsterBodyPart_OnSelected;
            MonsterCreatorManager.Instance.OnBodyChanged += MonsterCreatorManager_OnBodyChanged;
            MonsterCreatorManager.Instance.OnNewSkinElementChoosed += MonsterCreatorManager_OnBodyChanged;
            MonsterCreatorManager.Instance.OnSkinElementDisavailabled += MonsterCreatorManager_OnSkinElementDisavailabled;

            Hide();
        }
        private void MonsterCreatorManager_OnSkinElementDisavailabled(object sender, EventArgs e)
        {
            UpdateRequiredDiamondAmountText();
        }

        private void MonsterCreatorManager_OnBodyChanged(object sender, EventArgs e) => Hide();
       
        private void MonsterBodyPart_OnSelected(object sender, EventArgs e) => Hide();

       
        public void UpdateRequiredDiamondAmountText()
        {
            Show();
            requiredDiamondAmountText.SetText(MonsterCreatorManager.Instance.GetSelectedMonsterSkinElementPrice().ToString());

            if (MonsterCreatorManager.Instance.BuyingManager.CheckEnoughDiamond())
                requiredDiamondAmountText.color = Color.green;
            else
                requiredDiamondAmountText.color = Color.red;

        }

        private void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        private void OnDestroy()
        {
            MonsterBodyPart.OnSelected -= MonsterBodyPart_OnSelected;
        }
    }
}
