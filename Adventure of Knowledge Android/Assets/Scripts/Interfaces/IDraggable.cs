using System;
using System.Collections.Generic;


namespace AdventureOfKnowledge
{
    public interface IDraggable
    {
        public void Drag();
        public void Drop();  
    }

    public enum DragState
    {
        None = 0,
        IsDrag = 1,
        IsDrop = 2,
    }
}
