using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class MonsterBodyPart : MonoBehaviour, IInteractable
    {
        public static event EventHandler OnSelected;
        public static event EventHandler OnDisselected;

        private const float maxScaleStep = 4f;
        private const float minScaleStep = -4f;
        private const float percentageScale = 0.1f;
        
        [field: SerializeField] public TypeOfBodyPart TypeOfBodyPart { get;private set; }

        [SerializeField] private int skinElementIndex;
        [SerializeField] private int colorIndex;
        [SerializeField] private bool isFlip;

        private int scaleStep;
        private SpriteRenderer spriteRenderer;

        private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();
        
        public void Interact() 
        {
            if (IsBodyElement()) return;

            OnSelected?.Invoke(this, EventArgs.Empty);

           spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.4f);
        } 

        public void ScaleUp()
        {
            if (scaleStep >= maxScaleStep) return;

            scaleStep++;
            transform.localScale = new Vector3(transform.localScale.x + (percentageScale * transform.localScale.x), transform.localScale.y + (percentageScale * transform.localScale.y), 1);
        }

        public void ScaleDown()
        {
            if(scaleStep <= minScaleStep) return;

            scaleStep--;
            transform.localScale = new Vector3(transform.localScale.x - (percentageScale * transform.localScale.x), transform.localScale.y - (percentageScale * transform.localScale.y), 1);
        }

        public void Flip() 
        {
            isFlip = !isFlip;
            Rotate();
        } 
       
        private void Rotate()
        {
            float angle = isFlip ? 180 : 0;
            transform.eulerAngles = new Vector3(0, angle, 0);
        }

        public void DestroySelf()
        {
            OnDisselected?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }

        public void Disselected() 
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
            OnDisselected?.Invoke(this, EventArgs.Empty);
        } 

        public bool CanLayOnIt() => IsBodyElement() || IsHornElement();

        public Vector2 GetSpriteSize() => new Vector2(spriteRenderer.sprite.bounds.size.x * transform.localScale.x, 
            spriteRenderer.sprite.bounds.size.y * transform.localScale.y);

        public void SetSortingOrder(int layer) => spriteRenderer.sortingOrder = layer;

        public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;

        public int GetId() => skinElementIndex;

        public void SetId(int id) => skinElementIndex = id;

        public void SetColorIndex(int colorIndex) => this.colorIndex = colorIndex;

        public bool IsRightArmElement() => TypeOfBodyPart == TypeOfBodyPart.Arm && !isFlip;

        public bool IsBodyElement() => TypeOfBodyPart == TypeOfBodyPart.Body;

        public bool IsHornElement() => TypeOfBodyPart == TypeOfBodyPart.Horn;

        public BodyPartSaveData GetSaveData(Vector3 bodyPosition)
        {
            return new BodyPartSaveData
            {
                distanceFromBodyX = bodyPosition.x - transform.position.x,
                distanceFromBodyY = bodyPosition.y - transform.position.y,
                scale = transform.localScale.x,
                scaleStep = scaleStep,
                isFlip = isFlip,
                spriteIndex = skinElementIndex,
                colorIndex = colorIndex,
                bodyPart = TypeOfBodyPart,
                child = GetChildDataList(bodyPosition)
            };
        }

        private List<BodyPartSaveData> GetChildDataList(Vector3 bodyPosition)
        {
            List<BodyPartSaveData> childList = new List<BodyPartSaveData>();

            foreach (Transform item in transform)
            {
                if (item.TryGetComponent(out MonsterBodyPart monsterBodyPart))
                    childList.Add(monsterBodyPart.GetSaveData(bodyPosition));
            }

            return childList;
        }

        public void Initialize(BodyPartSaveData bodyPartSaveData, MonsterVisual monsterVisual, Transform parentTransform)
        {
            MonsterSkinElementSO monsterSkinElementSO = monsterVisual.MonsterSkinElementListSO.GetMonsterSkinElementFromBodyPart(bodyPartSaveData.bodyPart);
            float posX = monsterVisual.GetBodyPosition().x - bodyPartSaveData.distanceFromBodyX;
            float posY = monsterVisual.GetBodyPosition().y - bodyPartSaveData.distanceFromBodyY;
            float posZ = monsterSkinElementSO.LayerLevel * -1;
            transform.parent = parentTransform;

            if(bodyPartSaveData.scale != 0)
                transform.localScale = new Vector3(bodyPartSaveData.scale, bodyPartSaveData.scale);

            transform.position = new Vector3(posX, posY, posZ);
         
            scaleStep = bodyPartSaveData.scaleStep;
            isFlip = bodyPartSaveData.isFlip;
            Rotate();
            
            skinElementIndex = bodyPartSaveData.spriteIndex;
            colorIndex = bodyPartSaveData.colorIndex;
            TypeOfBodyPart = bodyPartSaveData.bodyPart;

            SetSprite(monsterSkinElementSO.GetMonsterSkinElemntColorVaraint(skinElementIndex, colorIndex));
            SetSortingOrder(monsterSkinElementSO.LayerLevel);
        }
    }

}

