
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AdventureOfKnowledge
{
    public class MonsterCreatorManager : MonoBehaviour
    {
        public static MonsterCreatorManager Instance {  get; private set; }

        public event EventHandler<OnColorSelectedEventArgs> OnColorSelected;
        public event EventHandler<OnBodyChangedEventArgs> OnBodyChanged;
        public event EventHandler<OnNewSkinElementChoosedEventArgs> OnNewSkinElementChoosed;
        public event EventHandler OnSkinElementDisavailabled;

        public class OnColorSelectedEventArgs : EventArgs { public int selectedColor; }
        public class OnBodyChangedEventArgs:EventArgs { public int skinInex; public int colorIndex; }
        public class OnNewSkinElementChoosedEventArgs: EventArgs { public Sprite newSkinElementSprite; }

        public const float monsterHeightMax = 3.5f;
        public int SelectedColor { get; private set; }
        public int SelectedSkinElement { get; private set; }
        public TypeOfBodyPart SelectedTypeOfBodyPart { get; private set; }
        public MonsterBodyPart SelectedMonsterBodyPart { get; private set; }
        public BuyingManager BuyingManager { get; private set; }

        [field: SerializeField] public MonsterVisualCreator MonsterVisualCreator { get; private set; }
        [field: SerializeField] public MonsterSkinElementListSO MonsterSkinElementList { get; private set; }

        [SerializeField] private MonsterBodyPart monsterBodyPartPrefab;
        [SerializeField] private List<Color> monsterSkinColors;

        [SerializeField] private LayerMask interactLayerMask;

        [SerializeField] private AvailableMonsterSkinElementList availableMonsterSkinElementList;

        private void Awake() 
        {
            Instance = this;

            SelectedTypeOfBodyPart = TypeOfBodyPart.Body;


            SaveManager.LoadAvailableSkinElement((callback) =>
            {
                if (callback.Value != null)
                    JsonUtility.FromJsonOverwrite(callback.Value.ToString(), availableMonsterSkinElementList);
     
            });
        }
       
        private void Start()
        {
            BuyingManager = new BuyingManager(this);

            MonsterBodyPart.OnSelected += MonsterBodyPart_OnSelected;
            MonsterBodyPart.OnDisselected += MonsterBodyPart_OnDisselected;
        }


        private void MonsterBodyPart_OnDisselected(object sender, EventArgs e) => SelectedMonsterBodyPart = null;

        private void MonsterBodyPart_OnSelected(object sender, EventArgs e) 
        {
            MonsterBodyPart monsterBodyPart = sender as MonsterBodyPart;

            if (!monsterBodyPart.IsBodyElement())
            {
                SelectedMonsterBodyPart?.Disselected();
                SelectedMonsterBodyPart = monsterBodyPart;
            }          
        } 
        
        public Color GetMonsterSkinColorFromIndex(int index) => monsterSkinColors[index];

        public int GetMonsterSkinColorCount() => monsterSkinColors.Count;

        public void SelectColor(int newSelectedColor)
        {
            SelectedColor = newSelectedColor;

            if (SelectedMonsterBodyPart)
            {
                SelectedMonsterBodyPart.SetSprite(GetMonsterSkinElementSO(SelectedMonsterBodyPart.TypeOfBodyPart).GetMonsterSkinElemntColorVaraint(SelectedMonsterBodyPart.GetId(), SelectedColor));
                SelectedMonsterBodyPart.SetColorIndex(SelectedColor);
            }
                
            OnColorSelected?.Invoke(this, new OnColorSelectedEventArgs { selectedColor = SelectedColor });
        }

        public void SelectSkinElement(int monsterSkinElement) 
        {
            SelectedSkinElement = monsterSkinElement;

            if(SelectedMonsterBodyPart)
                SelectedMonsterBodyPart.Disselected();

            if(BuyingManager.CheckSkinElementAvailable(monsterSkinElement, SelectedTypeOfBodyPart))
                UpdateNewElementVisual();
            else
                OnSkinElementDisavailabled?.Invoke(this, EventArgs.Empty);
        }

        public void SelectTypeOfBody(TypeOfBodyPart typeOfBodyPart) => SelectedTypeOfBodyPart = typeOfBodyPart;

        public MonsterSkinElementSO GetMonsterSkinElementSO(TypeOfBodyPart typeOfBodyPart) => MonsterSkinElementList.GetMonsterSkinElementFromBodyPart(typeOfBodyPart);

        public int GetSelectedMonsterSkinElementPrice() => GetMonsterSkinElementSO(SelectedTypeOfBodyPart).GetMonsterSkinElementPrice(SelectedSkinElement);

        public List<AvailableMonsterSkinElementSaveData> GetAvaibleSkinElement() => availableMonsterSkinElementList.monsterSkinElementSaveDatas;

        public AvailableMonsterSkinElementList GetMonsterSkinElementList() => availableMonsterSkinElementList;

        public void SpawnMonsterBodyPart(Vector3 position, Vector3 size) 
        {
            if(!CanPutBodyPart(position, size, out Transform parentTransform)) return;

            BodyPartSaveData bodyPartSaveData = new BodyPartSaveData()
            {
                distanceFromBodyX = MonsterVisualCreator.GetBodyPosition().x - position.x,
                distanceFromBodyY = MonsterVisualCreator.GetBodyPosition().y - position.y,
                colorIndex = SelectedColor,
                spriteIndex = SelectedSkinElement,
                bodyPart = SelectedTypeOfBodyPart
            };

            InstantiateMonsterBodyPart(bodyPartSaveData, parentTransform).Interact();
        }

        public MonsterBodyPart InstantiateMonsterBodyPart(BodyPartSaveData initializationData, Transform parentTransform)
        {
            MonsterBodyPart monsterBodyPart = Instantiate(monsterBodyPartPrefab);
            monsterBodyPart.Initialize(initializationData, MonsterVisualCreator, parentTransform);
            monsterBodyPart.gameObject.AddComponent<PolygonCollider2D>();
            return monsterBodyPart;
        }

        public bool CanPutBodyPart(Vector2 bodyPartPosition, Vector2 bodyPartSize, out Transform parentTransform)
        {
            parentTransform = MonsterVisualCreator.transform;

            if(CheckDistance(bodyPartPosition)) return false;

            Collider2D[] collider2D = Physics2D.OverlapBoxAll(bodyPartPosition, bodyPartSize, 0f, interactLayerMask);

            if (collider2D.Length == 0) return false;

            bool canPut = false;

            for (int i = 0; i < collider2D.Length; i++)
            {
                if (collider2D[i].TryGetComponent(out MonsterBodyPart DetectMonsterBodyPart))
                {
                    if (DetectMonsterBodyPart == SelectedMonsterBodyPart) continue;
                    if (DetectMonsterBodyPart.CanLayOnIt())
                    {
                        canPut = true;

                        if (DetectMonsterBodyPart.IsHornElement())
                            parentTransform = DetectMonsterBodyPart.transform;

                        break;
                    }
                }
            }
            return canPut;
        }

        private bool CheckDistance(Vector2 bodyPartPosition) => Vector2.Distance(bodyPartPosition, MonsterVisualCreator.GetBodyPosition()) > monsterHeightMax;

        public void UpdateNewElementVisual()
        {
            if (SelectedTypeOfBodyPart == TypeOfBodyPart.Body)
                UpdateBodyVisual();
            else
                OnNewSkinElementChoosed?.Invoke(this, new OnNewSkinElementChoosedEventArgs
                {
                    newSkinElementSprite = GetMonsterSkinElementSO(SelectedTypeOfBodyPart).GetMonsterSkinElemntColorVaraint(SelectedSkinElement, SelectedColor)
                });    
        }

        private void UpdateBodyVisual() 
        {
            OnBodyChanged?.Invoke(this, new OnBodyChangedEventArgs 
            {
                skinInex = SelectedSkinElement,
                colorIndex = SelectedColor
            });
        }

        private void SaveMonsterVisual()
        {
             List<BodyPartSaveData> bodyPartSaveDatas = new List<BodyPartSaveData>();

            foreach (Transform child in MonsterVisualCreator.transform)
            {
                MonsterBodyPart monsterBodyPart = child.GetComponent<MonsterBodyPart>();
                BodyPartSaveData bodyPartSaveData = monsterBodyPart.GetSaveData(MonsterVisualCreator.GetBodyPosition());
                bodyPartSaveDatas.Add(bodyPartSaveData);
            }

            SaveManager.SaveMonsterVisual(bodyPartSaveDatas);
        }

        public void SaveMonsterCreatorData()
        {
            SaveMonsterVisual();
            BuyingManager.SaveMonsterCreatorData();
        }

        private void OnDestroy()
        {
            MonsterBodyPart.OnSelected -= MonsterBodyPart_OnSelected;
            MonsterBodyPart.OnDisselected -= MonsterBodyPart_OnDisselected;
        }
    } 
}
