﻿using DataPersistence.Data;

namespace DataPersistence {
    public interface IGameDataDataPersistence {
        void LoadGame(GameData gameData);

        void SaveGame(ref GameData gameData);
        
    }
}