using UnityEngine;
using System.IO;

namespace Game
{
    public class FileManager : MonoBehaviour
    {
        [SerializeField] private TextAsset levelDataFile;
        [SerializeField] private TextAsset playerDataFile;
        [SerializeField] private TextAsset itemDataFile;

        public static FileManager Instance { get; set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        
        public void ReadData(Object dataManager, DataType type)
        {
            if (dataManager == null) return;
            
            string data;
            switch (type)
            {
                case DataType.Player:
                    if (!playerDataFile) return; 
                    data = File.ReadAllText(Application.dataPath + "/Resources/Jsons/" + playerDataFile.name + ".json");
                    // PlayerDataManager playerDataManager = dataManager as PlayerDataManager;
                    // playerDataManager.PlayerDataContainer = JsonUtility.FromJson<PlayerDataContainer>(data);
                    break;
                case DataType.Level:
                    if (!levelDataFile) return; 
                    data = File.ReadAllText(Application.dataPath + "/Resources/Jsons/" + levelDataFile.name + ".json");
                    // LevelDataManager levelDataManager = dataManager as LevelDataManager;
                    // levelDataManager.LevelDataContainer = JsonUtility.FromJson<LevelDataContainer>(data);
                    break;
                case DataType.Item:
                    if (!itemDataFile) return; 
                    data = File.ReadAllText(Application.dataPath + "/Resources/Jsons/" + itemDataFile.name + ".json");
                    // ItemDataManager itemDataManager = dataManager as ItemDataManager;
                    // itemDataManager.ItemDataContainer = JsonUtility.FromJson<ItemDataContainer>(data);
                    break;
            }
        }
        
        public void WriteData(Object dataManager, DataType type)
        {
            if (dataManager == null) return;
            
            switch (type)
            {
                case DataType.Player:
                    if (!playerDataFile) return;
                    // PlayerDataManager playerDataManager = dataManager as PlayerDataManager;
                    // WriteFile(playerDataManager.PlayerDataContainer, playerDataFile);
                    break;
                case DataType.Level:
                    if (!levelDataFile) return;
                    // LevelDataManager levelDataManager = dataManager as LevelDataManager;
                    // WriteFile(levelDataManager.LevelDataContainer, levelDataFile);
                    break;
                case DataType.Item:
                    if (!itemDataFile) return;
                    // ItemDataManager itemDataManager = dataManager as ItemDataManager;
                    // WriteFile(itemDataManager.ItemDataContainer, itemDataFile);
                    break;
            }

        }
        
        private T ReadFile<T>(TextAsset fileToRead)
        {
            return JsonUtility.FromJson<T>(fileToRead.text);
        }
        
        private void WriteFile(object dataType, TextAsset file)
        {
            string data = JsonUtility.ToJson(dataType);
            File.WriteAllText(Application.dataPath + "/Resources/Jsons/" + file.name + ".json", data);
        }
        
    }

    public enum DataType
    {
        Player,
        Level,
        Item
    }
}