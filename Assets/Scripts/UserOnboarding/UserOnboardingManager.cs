using System.Collections;
using DataPersistence;
using DataPersistence.Data;
using Firebase;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UserOnboarding {
    public class UserOnboardingManager : MonoBehaviour, IClientConfigDataPersistence {
        public static UserOnboardingManager instance;

        [Header("Menu Panels")] public GameObject loginPanel;
        public GameObject registerPanel;
        public GameObject emailVerificationPanel;
        public GameObject gameEntrancePanel;
        public Text emailVerificationText;
        public GameObject profileUpdatePanel;

        // Login variables
        [Space] [Header("Login")] public InputField emailLoginField;
        public InputField passwordLoginField;

        // Register variables
        [Space] [Header("Register")] public InputField usernameRegisterField;
        public InputField emailRegisterField;
        public InputField passwordRegisterField;
        public InputField passwordConfirmRegisterField;

        // Avatar Update
        [Space] [Header("Avatar")] public Image profileImage;
        public InputField urlInputField;

        [Space] [Header("Firebase Authentication Buttons")]
        public Button loginButton;

        public Button registerButton;
        public Button updateProfilePictureButton;
        public Button logOutButton;

        [Space] [Header("Other Settings")] public Toggle rememberMeToggle;
        private bool _loadingProfilePicture;

        private void Awake() {
            if (instance == null) instance = this;
        }

        private void Start() {
            OpenLoginPanel();
            loginButton.onClick.AddListener(() => { FirebaseAuthManager.Instance.Login(); });
            registerButton.onClick.AddListener(() => { FirebaseAuthManager.Instance.Register(); });
            updateProfilePictureButton.onClick.AddListener(() => { FirebaseAuthManager.Instance.UpdateProfilePicture(); });
            logOutButton.onClick.AddListener(() => { FirebaseAuthManager.Instance.Logout(); });
        }

        private void ClearUI() {
            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
            emailVerificationPanel.SetActive(false);
            gameEntrancePanel.SetActive(false);
            profileUpdatePanel.SetActive(false);
        }

        public void OpenLoginPanel() {
            ClearUI();
            ClearLoginInputFields();
            loginPanel.SetActive(true);
            Debug.Log("Opening login panel");
        }

        public void OpenRegisterPanel() {
            ClearUI();
            ClearRegisterInputFields();
            registerPanel.SetActive(true);
            Debug.Log("Open register panel");
        }

        public void OpenEmailVerificationResponse(bool isEmailSend, string emailId, string errorMessage) {
            ClearUI();
            emailVerificationPanel.SetActive(true);
            emailVerificationText.text = isEmailSend
                ? $"Please verify your email address\nVerification email has been sent to {emailId}"
                : $"Couldn't send email: {errorMessage}";
        }

        public void OpenProfilePictureUpdatePanel() {
            if (_loadingProfilePicture) return;
            ClearUI();
            profileUpdatePanel.SetActive(true);
            urlInputField.text = "";
            Debug.Log("Open profile picture update panel");
        }

        public void OpenGameEntrancePanel() {
            ClearUI();
            gameEntrancePanel.SetActive(true);
            Debug.Log("Open game entrance panel");
        }

        private void ClearLoginInputFields() {
            emailLoginField.text = "";
            passwordLoginField.text = "";
            rememberMeToggle.isOn = false;
        }

        private void ClearRegisterInputFields() {
            usernameRegisterField.text = "";
            emailRegisterField.text = "";
            passwordRegisterField.text = "";
            passwordConfirmRegisterField.text = "";
        }

        public void GoToGameScene() {
            if (_loadingProfilePicture) return;
            SceneManager.LoadScene("GameScene");
        }

        public void LoadProfilePicture(string url) {
            StartCoroutine(LoadProfilePictureIE(url));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoadProfilePictureIE(string url) {
            _loadingProfilePicture = true;

            // Loading User's profile picture        
            var uwr = UnityWebRequestTexture.GetTexture(url);
            yield return uwr.SendWebRequest();

            // if there is any error show it on console
            if (uwr.result == UnityWebRequest.Result.ConnectionError
                || uwr.result == UnityWebRequest.Result.ProtocolError
                || uwr.result == UnityWebRequest.Result.DataProcessingError) {
                Debug.Log(uwr.error);
            }
            else {
                // Set profileImage 
                var texture = DownloadHandlerTexture.GetContent(uwr);
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                profileImage.sprite = sprite;
            }

            _loadingProfilePicture = false;
        }

        public bool IsloadingProfilePicture() {
            return _loadingProfilePicture;
        }

        public string GetProfilePictureURL() {
            return urlInputField.text;
        }

        public void LoadClientConfig(ClientConfig clientConfig) {
            rememberMeToggle.isOn = clientConfig.rememberLoginData;
            emailLoginField.text = clientConfig.email;
            passwordLoginField.text = clientConfig.password;
            
            // Auto login here
            if (rememberMeToggle.isOn) StartCoroutine(FirebaseAuthManager.Instance.CheckForAutoLogin());
        }

        public void SaveClientConfig(ref ClientConfig clientConfig) {
            clientConfig.rememberLoginData = rememberMeToggle.isOn;
            clientConfig.email = clientConfig.rememberLoginData ? emailLoginField.text : "";
            clientConfig.password = clientConfig.rememberLoginData ? passwordLoginField.text : "";
        }
    }
}