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

        private const string SAVE_BODY_PARTS_KEY = "/BodyParts";

        private const string SAVE_AVAILABLE_SKIN_ELEMENT_KEY = "/AvailableSkinElement";
        private const string PLAYER_PREFS_NAME = "PlayerName";
        private const string PLAYER_PREFS_BEST_SCORE = "BestScore";
        private const string PLAYER_PREFS_DIAMOND_AMOUNT = "DiamondAmount";

        private const string DATA_SPIN_DATE = "SpinDate";
        private const string DATA_RENEW_SPIN_DATE = "RenewSpinDate";

        private const string DATA_BASE_NAME = "Users";

        private static DatabaseReference databaseReference;
        private static string userId;

        public static void InitializeDatabase()
        {
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            userId = SystemInfo.deviceUniqueIdentifier;
        }

        private static void SaveData<T>(T data, string dataName)
        {
            databaseReference.Child(DATA_BASE_NAME).Child(userId).Child(dataName).SetValueAsync(data);
        }

        private static void SaveJsonData<T>(T data, string dataName)
        {
            string json = JsonUtility.ToJson(data);
            SaveData(json, dataName);
        }

        public static void SaveSpinTime(CurrentDate currentDate)
        {
            SaveJsonData(currentDate, DATA_SPIN_DATE);
        }

        public static void SaveRenewSpinTime(CurrentDate currentDate)
        {
            SaveJsonData(currentDate, DATA_RENEW_SPIN_DATE);
        }

        public static void SavePlayerName(string playerName)
        {
            SaveData(playerName, PLAYER_PREFS_NAME);
        }

        public static void SaveTheBestLevelScore(int gameScore, DifficultyLevel difficultyLevel, string gameScene)
        {
            SaveData(gameScore, gameScene.ToString() + difficultyLevel.ToString() + PLAYER_PREFS_BEST_SCORE);
        }

        public static void SaveDiamondAmount(int diamondAmount)
        {
            SaveData(diamondAmount, PLAYER_PREFS_DIAMOND_AMOUNT);
        }

        public static void SaveAvailableSkinElement(AvailableMonsterSkinElementList monsterSkinElementSaveDatas)
        {
            SaveJsonData(monsterSkinElementSaveDatas, SAVE_AVAILABLE_SKIN_ELEMENT_KEY);
        }

        private static void LoadData(string dataName, Action<DataSnapshot> callback)
        {
            databaseReference.Child(DATA_BASE_NAME).Child(userId).Child(dataName).GetValueAsync().ContinueWithOnMainThread(task =>
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
            LoadData(DATA_SPIN_DATE, callback);
        }

        public static void LoadRenewSpinTime(Action<DataSnapshot> callback)
        {
            LoadData(DATA_RENEW_SPIN_DATE, callback);
        }

        public static void LoadPlayerName(Action<DataSnapshot> callback)
        {
            LoadData(PLAYER_PREFS_NAME, callback);
        }

        public static void LoadTheBestLevelScore(DifficultyLevel difficultyLevel, string gameScene, Action<DataSnapshot> callback) 
        {
            LoadData(gameScene.ToString() + difficultyLevel.ToString() + PLAYER_PREFS_BEST_SCORE, callback);
        } 

        public static void LoadDiamondAmount(Action<DataSnapshot> callback) 
        {
            LoadData(PLAYER_PREFS_DIAMOND_AMOUNT, callback);
        }

        public static void LoadAvailableSkinElement(Action<DataSnapshot> callback) 
        {
            LoadData(SAVE_AVAILABLE_SKIN_ELEMENT_KEY, callback);
        }

       

        public static void ResetSpinTime()
        {
            databaseReference.Child(DATA_BASE_NAME).Child(userId).Child(DATA_SPIN_DATE).RemoveValueAsync();
        }

        public static List<BodyPartSaveData> LoadMonsterVisual() => DeserializeData<List<BodyPartSaveData>>(SAVE_BODY_PARTS_KEY);

        public static void SaveMonsterVisual(List<BodyPartSaveData> bodyPartSaveDatas) => SerializeData(bodyPartSaveDatas, SAVE_BODY_PARTS_KEY);

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
