using System;
using System.Collections.Generic;
using UnityEngine;


namespace AdventureOfKnowledge.LabyrinthGame
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;

        private List<PathNode> grid;

        private int width;
        private int height;

        private List<PathNode> openList;
        private List<PathNode> closedList;

        public Pathfinding(int width, int height) 
        {
            this.width = width;
            this.height = height;
            grid = new List<PathNode>();
            closedList = new List<PathNode>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    PathNode pathNode = new PathNode(j, i);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;

                    grid.Add(pathNode);
                }
            }

        }

        public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = GetPathNodeFromPosition(startX, startY);
            PathNode endNode = GetPathNodeFromPosition(endX, endY);

            openList = new List<PathNode>{startNode};
            
            startNode.gCost = 0;
            startNode.hCost = CalculateDistance(startNode, endNode);
            startNode.CalculateFCost();

            while(openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if(currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;

                    int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode,neighbourNode);
                    if(tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistance(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if(!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }

                }

            }
            return null;
            
        }

        private int CalculateDistance(PathNode a, PathNode b)
        {
            int xDistance = Math.Abs(a.positionX - b.positionX);
            int yDistance = Math.Abs(a.positionY - b.positionY);
            int remaining = Math.Abs(xDistance - yDistance);
            return remaining * MOVE_STRAIGHT_COST;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostNode = pathNodeList[0];
            for(int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost)
                    lowestFCostNode = pathNodeList[i];
            }
            return lowestFCostNode;
        }

        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> pathNodes = new List<PathNode>();
            pathNodes.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
                pathNodes.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            pathNodes.Reverse();
            return pathNodes;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            if(currentNode.positionX - 1 >= 0)
                neighbourList.Add(GetPathNodeFromPosition(currentNode.positionX - 1,currentNode.positionY));

            if(currentNode.positionX + 1 < width)
                neighbourList.Add(GetPathNodeFromPosition(currentNode.positionX + 1, currentNode.positionY));

            if(currentNode.positionY - 1 >= 0)
                neighbourList.Add(GetPathNodeFromPosition(currentNode.positionX, currentNode.positionY - 1));

            if(currentNode.positionY + 1 < height)
                neighbourList.Add(GetPathNodeFromPosition(currentNode.positionX,currentNode.positionY + 1));

            return neighbourList;

        }
       

        public PathNode GetPathNodeFromPosition(int  x, int y)
        {
            foreach (var node in grid)
            {
                if (node.positionX == x && node.positionY == y)
                    return node;
            }
            return default;
        }

    }
}
