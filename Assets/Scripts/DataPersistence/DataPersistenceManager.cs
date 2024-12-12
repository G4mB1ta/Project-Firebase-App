using System.Collections.Generic;
using System.Linq;
using DataPersistence.Data;
using UnityEngine;

namespace DataPersistence {
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string dateFileName = "";
        
        private GameData _gameData;
        public static DataPersistenceManager instance;
        private List<IDataPersistence> _dataPersistenceObjects;
        private FileDataHandler _dataHandler;
        private void Awake() {
            if (instance != null) {
                Debug.LogWarning("More than one DataPersistenceManager in scene.");
            }
            instance = this;
        }
        private void Start() {
            this._dataHandler = new FileDataHandler(Application.persistentDataPath, dateFileName);
            this._dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void NewGame() {
            _gameData = new GameData();
        }

        public void LoadGame() {
            // Load gameData from file or DB
            this._gameData = _dataHandler.Load();
            // if null then create new gameData
            if (_gameData == null) {
                Debug.LogWarning("No data was found. Initializing new game.");
                NewGame();
            }
            // push data to all scripts that need 
            foreach (var dataPersistenceObject in _dataPersistenceObjects) {
                dataPersistenceObject.LoadGame(_gameData);
            }
        }

        public void SaveGame() {
            foreach (var dataPersistenceObject in _dataPersistenceObjects) {
                dataPersistenceObject.SaveGame(ref _gameData);
            }
            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit() {
            SaveGame();
        }

        private static List<IDataPersistence> FindAllDataPersistenceObjects() {
            // Load all IDataPersistenceObjs from scene
            IEnumerable<IDataPersistence> dataPersistenceObjectsIE =
                FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return dataPersistenceObjectsIE.ToList();
        }
    }
}
