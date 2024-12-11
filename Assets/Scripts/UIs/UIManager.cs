using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    [Header("Menu Panels")] 
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject emailVerificationPanel;
    [SerializeField] private GameObject gameEntrancePanel;
    [SerializeField] private Text emailVerificationText;
    [SerializeField] private GameObject profileUpdatePanel;

    // Login variables
    [Space] [Header("Login")] 
    [SerializeField] private InputField emailLoginField;
    [SerializeField] private InputField passwordLoginField;

    // Register variables
    [Space] [Header("Register")] 
    [SerializeField] private InputField usernameRegisterField;
    [SerializeField] private InputField emailRegisterField;
    [SerializeField] private InputField passwordRegisterField;
    [SerializeField] private InputField passwordConfirmRegisterField;

    // Avatar Update
    [Space] [Header("Avatar")] 
    [SerializeField] private Image profileImage;
    [SerializeField] private InputField urlInputField;
    
    private bool _loadingProfilePicture = false;
    
    private void Awake() {
        if (instance == null) instance = this;
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
    }

    public void OpenRegisterPanel() {
        ClearUI();
        ClearRegisterInputFields();
        registerPanel.SetActive(true);
    }

    public void OpenEmailVerificationResponse(bool isEmailSend, string emailId, string errorMessage) {
        ClearUI();
        emailVerificationPanel.SetActive(true);
        emailVerificationText.text = isEmailSend ? $"Please verify your email address\nVerification email has been sent to {emailId}" : $"Couldn't send email: {errorMessage}";
    }

    public void OpenProfilePictureUpdatePanel() {
        if (_loadingProfilePicture) return;
        ClearUI();
        profileUpdatePanel.SetActive(true);
        urlInputField.text = "";
    }

    public void OpenGameEntrancePanel() {
        ClearUI();
        gameEntrancePanel.SetActive(true);
    }

    private void ClearLoginInputFields() {
        emailLoginField.text = "";
        passwordLoginField.text = "";
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
}