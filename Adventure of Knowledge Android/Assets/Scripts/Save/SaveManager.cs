using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;


namespace AdventureOfKnowledge
{
    public static class SaveManager
    {
        public static event Action OnLoadCompleted;

        private const string SAVES_PATH = "/saves";

        private const string BODY_PARTS_KEY = "/BodyParts";
        private const string AVAILABLE_SKIN_ELEMENT_KEY = "/AvailableSkinElement";
        private const string PLAYER_NAME_KEY = "PlayerName";
        private const string BEST_SCORE_KEY = "BestScore";
        private const string DIAMOND_AMOUNT_KEY = "DiamondAmount";
        private const string SPIN_DATE_KEY = "SpinDate";
        private const string RENEW_SPIN_DATE_KEY = "RenewSpinDate";

        private const string DATA_BASE_NODE_NAME = "Users";

        private static DatabaseReference databaseReference;
        private static string userId;

        public static void InitializeDatabase()
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            userId = SystemInfo.deviceUniqueIdentifier;
        }

        private static void SaveData<T>(T data, string dataName)
        {
            databaseReference.Child(DATA_BASE_NODE_NAME).Child(userId).Child(dataName).SetValueAsync(data);
        }

        private static void SaveJsonData<T>(T data, string dataName)
        {
            string json = JsonUtility.ToJson(data);
            SaveData(json, dataName);
        }

        public static void SaveSpinTime(CurrentDate currentDate)
        {
            SaveJsonData(currentDate, SPIN_DATE_KEY);
        }

        public static void SaveRenewSpinTime(CurrentDate currentDate)
        {
            SaveJsonData(currentDate, RENEW_SPIN_DATE_KEY);
        }

        public static void SavePlayerName(string playerName)
        {
            SaveData(playerName, PLAYER_NAME_KEY);
        }

        public static void SaveTheBestLevelScore(int gameScore, DifficultyLevel difficultyLevel, string gameScene)
        {
            SaveData(gameScore, gameScene.ToString() + difficultyLevel.ToString() + BEST_SCORE_KEY);
        }

        public static void SaveDiamondAmount(int diamondAmount)
        {
            SaveData(diamondAmount, DIAMOND_AMOUNT_KEY);
        }

        public static void SaveAvailableSkinElement(AvailableMonsterSkinElementList monsterSkinElementSaveDatas)
        {
            SaveJsonData(monsterSkinElementSaveDatas, AVAILABLE_SKIN_ELEMENT_KEY);
        }

        private static void LoadData(string dataName, Action<DataSnapshot> callback)
        {
            databaseReference.Child(DATA_BASE_NODE_NAME).Child(userId).Child(dataName).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    OnLoadCompleted?.Invoke();
                    callback(task.Result);
                }
                    
            });
        }

        public static void LoadSpinTime(Action<DataSnapshot> callback)
        {
            LoadData(SPIN_DATE_KEY, callback);
        }

        public static void LoadRenewSpinTime(Action<DataSnapshot> callback)
        {
            LoadData(RENEW_SPIN_DATE_KEY, callback);
        }

        public static void LoadPlayerName(Action<DataSnapshot> callback)
        {
            LoadData(PLAYER_NAME_KEY, callback);
        }

        public static void LoadTheBestLevelScore(DifficultyLevel difficultyLevel, string gameScene, Action<DataSnapshot> callback) 
        {
            LoadData(gameScene.ToString() + difficultyLevel.ToString() + BEST_SCORE_KEY, callback);
        } 

        public static void LoadDiamondAmount(Action<DataSnapshot> callback) 
        {
            LoadData(DIAMOND_AMOUNT_KEY, callback);
        }

        public static void LoadAvailableSkinElement(Action<DataSnapshot> callback) 
        {
            LoadData(AVAILABLE_SKIN_ELEMENT_KEY, callback);
        }

       

        public static void ResetSpinTime()
        {
            databaseReference.Child(DATA_BASE_NODE_NAME).Child(userId).Child(SPIN_DATE_KEY).RemoveValueAsync();
        }

        public static List<BodyPartSaveData> LoadMonsterVisual() => DeserializeData<List<BodyPartSaveData>>(BODY_PARTS_KEY);

        public static void SaveMonsterVisual(List<BodyPartSaveData> bodyPartSaveDatas) => SerializeData(bodyPartSaveDatas, BODY_PARTS_KEY);

        private static void SerializeData<T>(T obj, string key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SAVES_PATH;
            Directory.CreateDirectory(path);

            FileStream file = new FileStream(path + key, FileMode.OpenOrCreate);

            binaryFormatter.Serialize(file, obj);

            file.Close();

        }

        private static T DeserializeData<T>(string key)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string path = Application.persistentDataPath + SAVES_PATH;
            T obj = default;

            if (File.Exists(path + key)) 
            {
                FileStream file = new FileStream(path + key, FileMode.Open);

                obj = (T)binaryFormatter.Deserialize(file);

                file.Close();
            }

            return obj;
        }  
    }
}
