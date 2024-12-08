using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject emailVerificationPanel;
    [SerializeField] private Text emailVerificationText;

    private void Awake() {
        if (instance == null) instance = this;
    }

    private void ClearUI() {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
    }

    public void OpenLoginPanel() {
        ClearUI();
        loginPanel.SetActive(true);
    }

    public void OpenRegisterPanel() {
        ClearUI();
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
}