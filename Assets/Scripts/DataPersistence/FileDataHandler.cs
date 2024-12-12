using System;
using System.IO;
using DataPersistence.Data;
using UnityEngine;

namespace DataPersistence {
    public class FileDataHandler {
        private readonly string _dataDirPath;
        private readonly string _dataFileName;

        public FileDataHandler(string dataDirPath, string dataFileName) {
            this._dataDirPath = dataDirPath;
            this._dataFileName = dataFileName;
        }

        public GameData Load() {
            string dataFilePath = Path.Combine(this._dataDirPath, this._dataFileName);
            GameData loadedGameData = null;

            if (File.Exists(dataFilePath)) {
                try {
                    string dataJson = File.ReadAllText(dataFilePath);
                    
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
            }
            
            return loadedGameData;
        }

        public void Save(GameData data) {
            // Combine to dataPath 
            string dataFilePath = Path.Combine(this._dataDirPath, _dataFileName);

            try {
                // create the dir the file will be written to if it doesn't already exist 
                Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath) ?? throw new InvalidOperationException());
                
                // serialize the C# game data object into json
                string dataJson = JsonUtility.ToJson(data, true);
                
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