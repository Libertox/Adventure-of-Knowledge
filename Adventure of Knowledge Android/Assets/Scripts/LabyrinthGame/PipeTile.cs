using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdventureOfKnowledge.LabyrinthGame
{
    public class PipeTile:MonoBehaviour,IPointerClickHandler
    {
        [Flags]
        public enum PipeType
        {
            Straight = 2,
            Angle = 4
        }

        public enum Direction
        {
            Up = 3,
            Right = 2,
            Down = 1,
            Left = 0,
            Max = 4
        }

        public List<Direction> FillDirectionList { get; private set; } = new List<Direction>();
        [field: SerializeField] public List<Direction> Directions { get; private set; } = new List<Direction>();

        [SerializeField] private PipeTileVisual pipeTileVisual;
        [SerializeField] private PipeType pipeType;

        [SerializeField] private bool isConstantFilled;
        [SerializeField] private bool canRotate;

        private bool isFill;
      
        private int gridPositionX;
        private int gridPositionY;

        private Coroutine fillCoroutine;

        public void Initialize(int gridPositionX, int gridPositionY)
        {
            SetupGridPosition(gridPositionX, gridPositionY);

            if(canRotate) SetRandomRotation();

            StartCoroutine(pipeTileVisual.AppearCoroutine());
        }

        private void SetupGridPosition(int gridPositionX, int gridPositionY)
        {
            this.gridPositionX = gridPositionX;
            this.gridPositionY = gridPositionY;
        }

        private void SetRandomRotation()
        {
            int randomRotateCoefficient = UnityEngine.Random.Range(0, 4);
            transform.eulerAngles = new Vector3(0, 0, randomRotateCoefficient * 90);

            for (int i = 0; i < randomRotateCoefficient; i++)
                ChangeDirection();
        }

        private void ChangeDirection()
        {
            for (int i = 0; i < Directions.Count; i++)
            {
                int dir = (int)Directions[i];
                dir++;

                if (dir == (int)Direction.Max) dir = 0;
                Directions[i] = (Direction)dir;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!canRotate) return;

            SoundManager.Instance.PlayInteractSound();

            Rotate();
            LabyrinthGameManager labyrinthGameManager = GameManager.Instance as LabyrinthGameManager;

            if (!isConstantFilled)
            {
                labyrinthGameManager.OutflowWaterNeighbourPipes(gridPositionX, gridPositionY);

                labyrinthGameManager.CheckNeighborsPipeIsFill(gridPositionX, gridPositionY);

                if (!CheckFillDirection())
                    Unfill(FillDirectionList[0]);
            }
            if (IsFilled())
                labyrinthGameManager.FlowWaterNeighbourPipes(gridPositionX, gridPositionY);

            labyrinthGameManager.IncreaseMoveNumber();
        }

        public void Rotate()
        {
            transform.Rotate(new Vector3(0, 0, 90f));
            ChangeDirection();
        }

        private bool CheckFillDirection()
        {
            if (FillDirectionList.Count == 0) return true;

            foreach (Direction filldirection in FillDirectionList)
            {
                if (Directions.Contains(filldirection))
                    return true;
            }
            return false;
        }

        public void Fill(Direction fillDirection, Action action)
        {
            isFill = true;
            FillDirectionList.Add(fillDirection);
            fillCoroutine = StartCoroutine(pipeTileVisual.FillCoroutine(fillDirection,action));
            
        }

        public void Unfill(Direction fillDirection)
        {
            if (isConstantFilled) return;

            isFill = false;
            StartCoroutine(pipeTileVisual.UnfillCoroutine(fillDirection));
            FillDirectionList.Clear();
        }

        public void StopFillCorotuine() 
        {
            if(fillCoroutine != null)
            {
                StopCoroutine(fillCoroutine);
                pipeTileVisual.SetFillAmount(0f);
            }             
        }  

        public bool IsFilled() => isFill || isConstantFilled;

        public bool HasPipeType(PipeType type) => pipeType.HasFlag(type);
    }
}
