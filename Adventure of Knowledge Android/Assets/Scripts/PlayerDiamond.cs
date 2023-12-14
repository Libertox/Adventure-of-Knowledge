using System;
using System.Collections.Generic;


namespace AdventureOfKnowledge
{
    public class PlayerDiamond
    {
        public static event EventHandler<OnDiamondChangedEventArgs> OnDiamondChanged;

        public class OnDiamondChangedEventArgs : EventArgs { public int diamondAmount; }

        private int currentDiamond;

        public PlayerDiamond(int diamondAmount) => AddDiamond(diamondAmount);
       
        public void AddDiamond(int diamondToAdd)
        {
            currentDiamond += diamondToAdd;

            OnDiamondChanged?.Invoke(this, new OnDiamondChangedEventArgs
            {
                diamondAmount = currentDiamond,
            });
        }

        public int GetDiamondAmount() => currentDiamond;

    }
}
