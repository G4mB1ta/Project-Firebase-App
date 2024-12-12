using UnityEngine.Serialization;

namespace DataPersistence.Data {
    [System.Serializable]
    public class GameData {
        public int level;
        public int exp;

        public int statPoints;
        public int healthPoints;
        public int strengthPoints;
        public int agilityPoints;
        public int intelligencePoints;
    
        public GameData() {
            level = 0;
            exp = 0;
            statPoints = 0;
            healthPoints = 0;
            strengthPoints = 0;
            agilityPoints = 0;
        }
    
    }
}
