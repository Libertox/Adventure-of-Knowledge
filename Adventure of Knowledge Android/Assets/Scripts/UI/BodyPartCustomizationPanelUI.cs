using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class BodyPartCustomizationPanelUI:MonoBehaviour
    {
        [SerializeField] private Button scaleUpButton;
        [SerializeField] private Button scaleDownButton;

        [SerializeField] private Button rotationButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button acceptButton;

        private void Awake()
        {

            scaleUpButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                MonsterCreatorManager.Instance.SelectedMonsterBodyPart.ScaleUp();
            });

            scaleDownButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                MonsterCreatorManager.Instance.SelectedMonsterBodyPart.ScaleDown();
            });

            rotationButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                MonsterCreatorManager.Instance.SelectedMonsterBodyPart.Flip();
            });

            deleteButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                MonsterCreatorManager.Instance.SelectedMonsterBodyPart.DestroySelf();
            });

            acceptButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                MonsterCreatorManager.Instance.SelectedMonsterBodyPart.Disselected();
            });
        }

        private void Start()
        {
            Hide();

            MonsterBodyPart.OnSelected += MonsterBodyPart_OnSelected;
            MonsterBodyPart.OnDisselected += MonsterBodyPart_OnDisselected;
        }

        private void MonsterBodyPart_OnDisselected(object sender, EventArgs e) => Hide();

        private void MonsterBodyPart_OnSelected(object sender, EventArgs e) 
        {
           MonsterBodyPart monsterBodyPart = (MonsterBodyPart)sender;

           if(!monsterBodyPart.IsBodyElement())
                Show();
        } 
      
        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void OnDestroy()
        {
            MonsterBodyPart.OnSelected -= MonsterBodyPart_OnSelected;
            MonsterBodyPart.OnDisselected -= MonsterBodyPart_OnDisselected;
        }

    }
}
