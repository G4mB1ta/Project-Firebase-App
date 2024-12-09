using Unity.VisualScripting;
using UnityEngine;
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
    
    // Login variables
    [Space] [Header("Login")] public InputField emailLoginField;
    public InputField passwordLoginField;

    // Register variables
    [Space] [Header("Register")] public InputField usernameRegisterField;
    public InputField emailRegisterField;
    public InputField passwordRegisterField;
    public InputField passwordConfirmRegisterField;
    
    private void Awake() {
        if (instance == null) instance = this;
    }

    private void Start() {
    }

    private void ClearUI() {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
        gameEntrancePanel.SetActive(false);
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

        if (isEmailSend) {
            emailVerificationText.text = $"Please verify your email address\nVerification email has been sent to {emailId}";            
        }
        else {
            emailVerificationText.text = $"Couldn't send email: {errorMessage}";
        }
    }

    public void OpenGameEntrancePanel() {
        ClearUI();
        gameEntrancePanel.SetActive(true);
    }

    public void ClearLoginInputFields() {
        emailLoginField.text = "";
        passwordLoginField.text = "";
    }

    public void ClearRegisterInputFields() {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordConfirmRegisterField.text = "";
    }

    public void GoToGameScene() {
        SceneManager.LoadScene("GameScene");
    }
}