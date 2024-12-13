using DataPersistence.Data;

namespace DataPersistence {
    public interface IClientConfigDataPersistence {
        void LoadClientConfig(ClientConfig clientConfig);
        void SaveClientConfig(ref ClientConfig clientConfig);
    }
}