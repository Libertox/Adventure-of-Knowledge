using System;
using System.Collections.Generic;


namespace AdventureOfKnowledge
{
    public class PlayerDiamond
    {
        public static event EventHandler<OnDiamondChangedEventArgs> OnDiamondChanged;

        public class OnDiamondChangedEventArgs : EventArgs { public int diamondAmount; }

        private int currentDiamond;

        public PlayerDiamond() 
        {
            SaveManager.LoadDiamondAmount((callback) =>
            {
                int diamondAmount = callback.Value == null ? 0 : int.Parse(callback.Value.ToString());
                AddDiamond(diamondAmount);
            });
        } 
       
        public void AddDiamond(int diamondToAdd)
        {
            currentDiamond += diamondToAdd;

            OnDiamondChanged?.Invoke(this, new OnDiamondChangedEventArgs
            {
                diamondAmount = currentDiamond,
            });

            SaveManager.SaveDiamondAmount(currentDiamond);
        }

        public int GetDiamondAmount() => currentDiamond;
    }
}
