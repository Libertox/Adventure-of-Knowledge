using System;
using System.Collections.Generic;
using UnityEngine;


namespace AdventureOfKnowledge
{
    public class BuyingManager
    {
        public event EventHandler OnNewSkinElementBought;

        private MonsterCreatorManager monsterCreatorManager;

        private PlayerDiamond playerDiamond;

        public BuyingManager(MonsterCreatorManager monsterCreatorManager) 
        {
            playerDiamond = new PlayerDiamond();
            this.monsterCreatorManager = monsterCreatorManager;
        }
        public bool CheckEnoughDiamond() => monsterCreatorManager.GetSelectedMonsterSkinElementPrice() < playerDiamond.GetDiamondAmount();

        public bool BuySkinElement()
        {
            if (!CheckEnoughDiamond()) return false;

            monsterCreatorManager.GetAvaibleSkinElement().Add(new AvailableMonsterSkinElementSaveData(monsterCreatorManager.SelectedSkinElement, monsterCreatorManager.SelectedTypeOfBodyPart));
            playerDiamond.AddDiamond(-monsterCreatorManager.GetSelectedMonsterSkinElementPrice());

            SoundManager.Instance.PlayBuySound();

            OnNewSkinElementBought?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public bool CheckSkinElementAvailable(int skinElementIndex, TypeOfBodyPart typeOfBodyPart)
        {
            foreach (AvailableMonsterSkinElementSaveData data in monsterCreatorManager.GetAvaibleSkinElement())
            {
                if (data.typeOfBodyPart == typeOfBodyPart && data.indexOnList == skinElementIndex)
                    return true;
            }
            return false;
        }

        public void SaveMonsterCreatorData()
        {
            SaveManager.SaveAvailableSkinElement(monsterCreatorManager.GetMonsterSkinElementList());
        }

    }
}
