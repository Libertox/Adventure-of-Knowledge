using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class MonsterCreatorUI:MonoBehaviour
    {
        [SerializeField] private Button backMainMenuButton;

        [SerializeField] private FadeIamgeUI fadeIamge;

        [SerializeField] private MonsterSkinElementUI monsterSkinElementTemplate;
        [SerializeField] private RectTransform skinElementsContainer;

        [SerializeField] private SelectedColorButtonUI selectedColorTemplate;
        [SerializeField] private RectTransform selectedColorContainer;

        [SerializeField] private Button bodyPartButtonTemplate;
        [SerializeField] private RectTransform bodyPartContainer;

        private void Awake()
        {
            SetupBodyPartButtons();

            backMainMenuButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                MonsterCreatorManager.Instance.SaveMonsterCreatorData();
                fadeIamge.FadeToBlack(() => SceneLoader.LoadScene(GameScene.MainMenu));
            });
        }

        private void SetupBodyPartButtons()
        {
            foreach (TypeOfBodyPart bodyPart in Enum.GetValues(typeof(TypeOfBodyPart)))
            {
                if (bodyPart == TypeOfBodyPart.Body) continue;

                Button bodyPartButton = Instantiate(bodyPartButtonTemplate, bodyPartContainer);

                MonsterSkinElementSO monsterSkinElementsSO = MonsterCreatorManager.Instance.MonsterSkinElementList.GetMonsterSkinElementFromBodyPart(bodyPart);

                bodyPartButton.onClick.AddListener(() => 
                {
                    SoundManager.Instance.PlayButtonSound();
                    MonsterCreatorManager.Instance.SelectTypeOfBody(bodyPart);
                    UpdateSkinElements(monsterSkinElementsSO);
                });
                bodyPartButton.GetComponent<Image>().sprite = monsterSkinElementsSO.GetMonsterSkinElemntColorVaraint(0, 0);
            }

            bodyPartButtonTemplate.onClick.AddListener(() => 
            {
                MonsterCreatorManager.Instance.SelectTypeOfBody(TypeOfBodyPart.Body);
                UpdateSkinElements(MonsterCreatorManager.Instance.MonsterSkinElementList.GetMonsterSkinElementFromBodyPart(TypeOfBodyPart.Body));
            });
        }

        private void Start()
        {
            UpdateSkinElements(MonsterCreatorManager.Instance.MonsterSkinElementList.GetMonsterSkinElementFromBodyPart(TypeOfBodyPart.Body));
            UpdateSelectedColor();

            MonsterCreatorManager.Instance.OnColorSelected += MonsterCreatorManager_OnColorSelected;
        }

        private void MonsterCreatorManager_OnColorSelected(object sender, MonsterCreatorManager.OnColorSelectedEventArgs e)
        {
            UpdateSkinElements(MonsterCreatorManager.Instance.GetMonsterSkinElementSO(MonsterCreatorManager.Instance.SelectedTypeOfBodyPart));
        }

        private void UpdateSkinElements(MonsterSkinElementSO monsterSkinElementsSO)
        {
            DeleteExistSkinElement(); 

            for (int i = 0; i < monsterSkinElementsSO.GetMonsterSkinElemntCount(); i++)
            {
                MonsterSkinElementUI monsterSkinElementUI = Instantiate(monsterSkinElementTemplate, skinElementsContainer);
                bool isAvailable = MonsterCreatorManager.Instance.BuyingManager.CheckSkinElementAvailable(i, monsterSkinElementsSO.BodyPart);
                monsterSkinElementUI.UpdateVisual(monsterSkinElementsSO, i);
                monsterSkinElementUI.SetAvailableSkinElement(isAvailable);
            }
        }

        private void DeleteExistSkinElement()
        {
            foreach (Transform skinElement in skinElementsContainer)
            {
                if (skinElement == monsterSkinElementTemplate.transform)
                    continue;

                Destroy(skinElement.gameObject);
            }
        }

        private void UpdateSelectedColor()
        {
            selectedColorTemplate.UpdateViusal(MonsterCreatorManager.Instance.GetMonsterSkinColorFromIndex(0), 0);
            for (int i = 1; i < MonsterCreatorManager.Instance.GetMonsterSkinColorCount(); i++)
            {
                SelectedColorButtonUI selectedColorUI = Instantiate(selectedColorTemplate, selectedColorContainer);
                selectedColorUI.UpdateViusal(MonsterCreatorManager.Instance.GetMonsterSkinColorFromIndex(i), i);
            }
        }

    }
}
