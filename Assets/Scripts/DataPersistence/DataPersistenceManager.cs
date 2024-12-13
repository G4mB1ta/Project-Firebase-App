using System;
using System.Collections.Generic;
using System.Linq;
using DataPersistence.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DataPersistence {
    public class DataPersistenceManager : MonoBehaviour {
        public static DataPersistenceManager Instance { get; private set; }

        [Header("File Storage Config")] [SerializeField]
        private string gameDataFileName = "";

        [SerializeField] private string clientConfigFileName = "";

        private GameData _gameData;
        private ClientConfig _clientConfig;
        private List<IGameDataDataPersistence> _gameDataPersistenceObjects;
        private List<IClientConfigDataPersistence> _clientConfigDataPersistenceObjects;
        private FileDataHandler _dataHandler;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
                Instance = this;
            else if (Instance != this) Destroy(gameObject);
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, gameDataFileName, clientConfigFileName);
            _gameDataPersistenceObjects = FindAllGameDataPersistenceObjects();
            _clientConfigDataPersistenceObjects = FindAlClientConfigDataPersistenceObjects();            
        }

        private void OnSceneUnloaded(Scene scene) {
            SaveGame();
            SaveClientConfig();
        }

        public void NewGame() {
            _gameData = new GameData();
            _clientConfig = new ClientConfig();
        }

        public void LoadGame() {
            // Load gameData from file or DB
            _gameData = _dataHandler.LoadGameData();
            // if null then create new gameData
            if (_gameData == null) {
                Debug.LogWarning("No game data was found. Initializing new game.");
                _gameData = new GameData();
            }

            // push data to all scripts that need 
            foreach (var dataPersistenceObject in _gameDataPersistenceObjects)
                dataPersistenceObject.LoadGame(_gameData);
        }

        public void LoadClientConfig() {
            _clientConfig = _dataHandler.LoadClientConfig();
            if (_clientConfig == null) {
                Debug.LogWarning("No client config was found. Initializing new client config.");
                _clientConfig = new ClientConfig();
            }

            foreach (var dataPersistenceObject in _clientConfigDataPersistenceObjects)
                dataPersistenceObject.LoadClientConfig(_clientConfig);
        }

        public void SaveGame() {
            foreach (var dataPersistenceObject in _gameDataPersistenceObjects)
                dataPersistenceObject.SaveGame(ref _gameData);
            _dataHandler.SaveGameData(_gameData);
        }

        // Call when unloaded scene & quiting application
        public void SaveClientConfig() {
            foreach (var dataPersistenceObject in _clientConfigDataPersistenceObjects)
                dataPersistenceObject.SaveClientConfig(ref _clientConfig);
            _dataHandler.SaveClientConfig(_clientConfig);
        }

        private static List<IGameDataDataPersistence> FindAllGameDataPersistenceObjects() {
            // Load all IDataPersistenceObjs from scene
            var dataPersistenceObjectsIE =
                FindObjectsOfType<MonoBehaviour>().OfType<IGameDataDataPersistence>();
            return dataPersistenceObjectsIE.ToList();
        }

        private static List<IClientConfigDataPersistence> FindAlClientConfigDataPersistenceObjects() {
            // Load all IDataPersistenceObjs from scene
            var dataPersistenceObjectsIE =
                FindObjectsOfType<MonoBehaviour>().OfType<IClientConfigDataPersistence>();
            return dataPersistenceObjectsIE.ToList();
        }
    }
}