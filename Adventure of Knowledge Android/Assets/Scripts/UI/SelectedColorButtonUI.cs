using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class SelectedColorButtonUI:MonoBehaviour
    {
        [SerializeField] private Button selectColorButton;
        [SerializeField] private Image selectedColorImage;
        [SerializeField] private GameObject selectedIndicator;

        private int selectedColorIndex;

        private void Awake() => selectColorButton.onClick.AddListener(() => 
        {
            SoundManager.Instance.PlayButtonSound();
            MonsterCreatorManager.Instance.SelectColor(selectedColorIndex);
        });
        

        private void Start()
        {
            MonsterCreatorManager.Instance.OnColorSelected += MonsterCreatorManager_OnColorSelected;

            UpdateSelectIndicator(0);
        }

        private void MonsterCreatorManager_OnColorSelected(object sender, MonsterCreatorManager.OnColorSelectedEventArgs e) => 
            UpdateSelectIndicator(e.selectedColor);
   

        private void UpdateSelectIndicator(int selectedColor)
        {
            if (selectedColor == selectedColorIndex)
                selectedIndicator.SetActive(true);
            else
                selectedIndicator.SetActive(false);
        }

        public void UpdateViusal(Color color, int selectedColorIndex)
        {
            selectedColorImage.color = color;
            this.selectedColorIndex = selectedColorIndex;
        }

    }
}
