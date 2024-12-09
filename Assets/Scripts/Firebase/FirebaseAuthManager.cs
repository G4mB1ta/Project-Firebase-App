using System;
using System.Collections;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Firebase {
    public class FirebaseAuthManager : MonoBehaviour {
        // Firebase variables
        [Header("Firebase")] public DependencyStatus dependencyStatus;
        private FirebaseAuth _auth;
        private FirebaseUser _user;

        // Login variables
        [Space] [Header("Login")] public InputField emailLoginField;
        public InputField passwordLoginField;

        // Register variables
        [Space] [Header("Register")] public InputField usernameRegisterField;
        public InputField emailRegisterField;
        public InputField passwordRegisterField;
        public InputField passwordConfirmRegisterField;

        private void Awake() {
            // Firebase Unity SDK for Android requires Google Play services
            StartCoroutine(CheckAndFixDependenciesAsync());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CheckAndFixDependenciesAsync() {
            Environment.SetEnvironmentVariable("USE_AUTH_EMULATOR", "localhost:9099");
            var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
            yield return new WaitUntil(() => dependencyTask.IsCompleted);

            dependencyStatus = dependencyTask.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                InitializeFirebase();
                yield return new WaitForEndOfFrame();
                // Check For Auto Login
                StartCoroutine(CheckForAutoLogin());
            }
            else {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        }

        private void InitializeFirebase() {
            Debug.Log("Setting up Firebase Auth");
            _auth = FirebaseAuth.DefaultInstance;
            // Register event when State change and Start Auth State with null
            _auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }

        private IEnumerator CheckForAutoLogin() {
            // If there is previous user
            if (_user != null) {
                // Reload before login
                var reloadUser = _user.ReloadAsync();

                // Auto Login after reload user info
                yield return new WaitUntil(() => reloadUser.IsCompleted);
                AutoLogin();
            }
            else {
                UIManager.instance.OpenLoginPanel();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void AutoLogin() {
            if (_user != null) {
                if (_user.IsEmailVerified) {
                    Reference.name = _user.DisplayName;
                    Debug.Log(Reference.name + " is now logged in.");
                    UIManager.instance.OpenGameEntrancePanel();
                }
                else {
                    SendEmailForVerification();
                }
            }
            else {
                UIManager.instance.OpenLoginPanel();
            }
        }

        private void AuthStateChanged(object sender, EventArgs eventArgs) {
            // If current user != previous user 
            if (_auth.CurrentUser != _user) {
                // If current user != previous user and current user != null
                // then signin 
                // If !signin and previous user != null and current user == null
                // then sign out
                var signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;
                if (!signedIn && _user != null) Debug.Log("Signed out + " + _user.UserId);

                // Change previous user to current user
                _user = _auth.CurrentUser;
                if (signedIn) Debug.Log("Signed in + " + _user.UserId);
            }
        }

        public void Login() {
            // Call login when user click login button, input data is input fields
            StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator LoginAsync(string email, string password) {
            var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            // If there is any error at login Task
            // then Debug to console
            // If there is no exception
            // then login
            if (loginTask.Exception != null) {
                Debug.LogError(loginTask.Exception);
                var firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
                var authError = (AuthError)firebaseException.ErrorCode;

                var failureMessage = "Login failed: ";
                failureMessage += authError switch {
                    AuthError.InvalidEmail => "Invalid email address",
                    AuthError.WrongPassword => "Wrong password",
                    AuthError.MissingEmail => "Email is missing",
                    AuthError.MissingPassword => "Password is missing",
                    _ => "Unknown Error"
                };

                Debug.LogError(failureMessage);
            }
            else {
                // Login Task successfully
                _user = loginTask.Result.User;
                Debug.Log($"{_user.DisplayName} logged in");

                if (_user.IsEmailVerified) {
                    Reference.name = _user.DisplayName;
                    UIManager.instance.OpenGameEntrancePanel();
                }
                else {
                    SendEmailForVerification();
                }
            }
        }

        public void Register() {
            StartCoroutine(RegisterAsync(usernameRegisterField.text, emailRegisterField.text,
                passwordRegisterField.text, passwordConfirmRegisterField.text));
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator RegisterAsync(string username, string email, string password, string confirmPassword) {
            // Missing username
            if (username == "") {
                Debug.Log("Name is empty");
            }
            // Missing email
            else if (email == "") {
                Debug.Log("Email is empty");
            }
            // password and confirmPassword do not match
            else if (password != confirmPassword) {
                Debug.Log("Passwords do not match");
            }
            else {
                var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
                yield return new WaitUntil(() => registerTask.IsCompleted);

                // If there is Exception at register Task
                if (registerTask.Exception != null) {
                    Debug.LogError(registerTask.Exception);
                    var firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                    var authError = (AuthError)firebaseException.ErrorCode;

                    var failureMessage = "Register failed: ";
                    failureMessage += authError switch {
                        AuthError.InvalidEmail => "Invalid email address",
                        AuthError.WrongPassword => "Wrong password",
                        AuthError.MissingEmail => "Email is missing",
                        AuthError.MissingPassword => "Password is missing",
                        _ => "Unknown Error"
                    };

                    Debug.LogError(failureMessage);
                }
                // Register successfully
                else {
                    _user = registerTask.Result.User;
                    var userProfile = new UserProfile { DisplayName = username };

                    var updateProfileTask = _user.UpdateUserProfileAsync(userProfile);
                    yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                    if (updateProfileTask.Exception != null) {
                        // Delete use if update profile is failed
                        _user.DeleteAsync();
                        Debug.LogError(updateProfileTask.Exception);
                        var firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                        var authError = (AuthError)firebaseException.ErrorCode;
                        var failureMessage = "Profile update failed: ";
                        failureMessage += authError switch {
                            AuthError.InvalidEmail => "Invalid email address",
                            AuthError.WrongPassword => "Wrong password",
                            AuthError.MissingEmail => "Email is missing",
                            AuthError.MissingPassword => "Password is missing",
                            _ => "Unknown Error"
                        };

                        Debug.LogError(failureMessage);
                    }
                    else {
                        // Update profile task successfully
                        Debug.Log($"{_user.DisplayName} registered successfully");
                        if (_user.IsEmailVerified)
                            UIManager.instance.OpenLoginPanel();
                        else
                            SendEmailForVerification();
                    }
                }
            }
        }

        private void SendEmailForVerification() {
            StartCoroutine(SendEmailForVerificationAsync());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator SendEmailForVerificationAsync() {
            if (_user != null) {
                var sendEmailTask = _user.SendEmailVerificationAsync();

                yield return new WaitUntil(() => sendEmailTask.IsCompleted);

                if (sendEmailTask.Exception != null) {
                    var firebaseException = sendEmailTask.Exception.GetBaseException() as FirebaseException;
                    var authError = (AuthError)firebaseException.ErrorCode;

                    var errorMessage = "Unknown Error: Please try again. ";
                    errorMessage += authError switch {
                        AuthError.Cancelled => "Email is cancelled",
                        AuthError.InvalidEmail => "Invalid email address",
                        AuthError.TooManyRequests => "Too Many Requests",
                        _ => "Unknown Error"
                    };
                    UIManager.instance.OpenEmailVerificationResponse(false, null, errorMessage);
                }
                else {
                    UIManager.instance.OpenEmailVerificationResponse(true, _auth.CurrentUser.Email, null);
                    Debug.Log("Email has successfully sent");
                }
            }
        }
    }
}