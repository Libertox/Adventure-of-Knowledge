using System;
using System.Collections.Generic;


namespace AdventureOfKnowledge
{
    public class PlayerDiamond
    {
        public static event EventHandler<OnDiamondChangedEventArgs> OnDiamondChanged;

        public class OnDiamondChangedEventArgs : EventArgs { public int diamondAmount; }

        public int CurrentDiamond { get; private set; }

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
            CurrentDiamond += diamondToAdd;

            OnDiamondChanged?.Invoke(this, new OnDiamondChangedEventArgs
            {
                diamondAmount = CurrentDiamond,
            });

            SaveManager.SaveDiamondAmount(CurrentDiamond);
        }
    }
}
