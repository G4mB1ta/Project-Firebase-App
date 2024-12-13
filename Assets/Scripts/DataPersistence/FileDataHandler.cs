using System;
using System.IO;
using DataPersistence.Data;
using UnityEngine;

namespace DataPersistence {
    public class FileDataHandler {
        private readonly string _dataDirPath;
        private readonly string _gameDataFileName;
        private readonly string _clientConfigFileName;

        public FileDataHandler(string dataDirPath, string gameDataFileName, string clientConfigFileName) {
            _dataDirPath = dataDirPath;
            _gameDataFileName = gameDataFileName;
            _clientConfigFileName = clientConfigFileName;
        }

        public GameData LoadGameData() {
            var dataFilePath = Path.Combine(_dataDirPath, _gameDataFileName);
            GameData loadedGameData = null;

            if (File.Exists(dataFilePath))
                try {
                    var dataJson = File.ReadAllText(dataFilePath);

                    // using (FileStream stream = new FileStream(dataFilePath, FileMode.Create)) {
                    //     using (StreamReader reader = new StreamReader(stream)) {
                    //         dataJson = reader.ReadToEnd();
                    //     }
                    // }
                    loadedGameData = JsonUtility.FromJson<GameData>(dataJson);
                }
                catch (Exception e) {
                    Debug.Log("Error occured while trying to load data from file: " + dataFilePath + "\n" + e);
                }

            return loadedGameData;
        }

        public ClientConfig LoadClientConfig() {
            var dataFilePath = Path.Combine(_dataDirPath, _clientConfigFileName);
            ClientConfig loadedClientConfig = null;

            if (File.Exists(dataFilePath))
                try {
                    var dataJson = File.ReadAllText(dataFilePath);

                    // using (FileStream stream = new FileStream(dataFilePath, FileMode.Create)) {
                    //     using (StreamReader reader = new StreamReader(stream)) {
                    //         dataJson = reader.ReadToEnd();
                    //     }
                    // }
                    loadedClientConfig = JsonUtility.FromJson<ClientConfig>(dataJson);
                }
                catch (Exception e) {
                    Debug.Log("Error occured while trying to load data from file: " + dataFilePath + "\n" + e);
                }

            return loadedClientConfig;
        }

        public void SaveGameData(GameData data) {
            // Combine to dataPath 
            var dataFilePath = Path.Combine(_dataDirPath, _gameDataFileName);

            try {
                // create the dir the file will be written to if it doesn't already exist 
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath) ?? throw new InvalidOperationException());

                // serialize the C# game data object into json
                var dataJson = JsonUtility.ToJson(data, true);

                File.WriteAllText(dataFilePath, dataJson);

                // using (FileStream stream = new FileStream(dataFilePath, FileMode.Create)) {
                //     using (StreamWriter writer = new StreamWriter(stream)) {
                //         writer.Write(dataJson);
                //     }
                // }
            }
            catch (Exception e) {
                Debug.Log("Error occured while trying to save data to file: " + dataFilePath + "\n" + e);
            }
        }

        public void SaveClientConfig(ClientConfig data) {
            // Combine to dataPath 
            var dataFilePath = Path.Combine(_dataDirPath, _clientConfigFileName);

            try {
                // create the dir the file will be written to if it doesn't already exist 
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath) ?? throw new InvalidOperationException());

                // serialize the C# game data object into json
                var dataJson = JsonUtility.ToJson(data, true);

                File.WriteAllText(dataFilePath, dataJson);

                // using (FileStream stream = new FileStream(dataFilePath, FileMode.Create)) {
                //     using (StreamWriter writer = new StreamWriter(stream)) {
                //         writer.Write(dataJson);
                //     }
                // }
            }
            catch (Exception e) {
                Debug.Log("Error occured while trying to save data to file: " + dataFilePath + "\n" + e);
            }
        }
    }
}