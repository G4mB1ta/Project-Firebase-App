namespace DataPersistence.Data {

    [System.Serializable]
    public class ClientConfig {
        public bool rememberLoginData;
        public string email;
        public string password;

        public ClientConfig() {
            rememberLoginData = false;
            email = "";
            password = "";
        }
    }
}
