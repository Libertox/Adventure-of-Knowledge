using System;
using System.Collections.Generic;


namespace AdventureOfKnowledge.LabyrinthGame
{
    public class PathNode
    {
        public int positionX;
        public int positionY;

        public int gCost;
        public int hCost;
        public int fCost;

        public PathNode cameFromNode;

        public PathNode(int positionX, int positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

    }
}
