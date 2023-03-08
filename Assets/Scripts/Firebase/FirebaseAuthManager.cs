using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class FirebaseAuthManager : MonoBehaviour
{
    #region Varibles
    // Firebase variable
    [Header("Firebase")]
    private DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;

    // Login Variables
    [Space]
    [Header("Login")]
    [SerializeField] InputField emailLoginField;
    [SerializeField] InputField passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    [SerializeField] InputField nameRegisterField;
    [SerializeField] InputField emailRegisterField;
    [SerializeField] InputField passwordRegisterField;
    [SerializeField] InputField confirmPasswordRegisterField;
    #endregion


    #region Initialization Methods
    private void Awake()
    {
        // Check that all of the necessary dependencies for firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    #endregion

    #region Unity Methods

    private void OnApplicationQuit()
    {
        auth.SignOut();
    }
    #endregion

    #region Login Methods
    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }
  /// <summary>
  /// Login Method, needs to be run in a coroutine
  /// </summary>
  /// <param name="email"></param>
  /// <param name="password"></param>
  /// <returns></returns>
    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);
        // Check if there are any errors
        if (loginTask.Exception != null)
        {
            // If there are errors, log them
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;
            // Get the error message
            string failedMessage = GetAuthErrorMessage("Login Failed! Because ", authError);

            Debug.Log(failedMessage);
        }
        else
        {
            user = loginTask.Result;

            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);
            // Refence Username and switch to GameScene
            References.UserName = user.DisplayName;
            References.UserID = user.UserId;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
        }
    }
    #endregion

    #region Registration Methods
    public void Register()
    {
        // StartCoroutine on the RegisterAsync
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }
#if UNITY_INCLUDE_TESTS
    public IEnumerator RegisterTestAsync(string name, string email, string password, string confirmPassword)
    {
        return RegisterAsync(name, email, password, confirmPassword);
    }
#endif
    /// <summary>
    /// RegisterAsync, registeres the user in Firebase
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="confirmPassword"></param>
    /// <returns></returns>
    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        // Check if Password and Password are the same, if not then LogError
        if (password != confirmPassword)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            // Create the user in firebase with Email and Password
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            // Yield until it's complted
            yield return new WaitUntil(() => registerTask.IsCompleted);
            // Check if theres any errors, if so then handle them,
            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = GetAuthErrorMessage("Registration Failed! Becuase ", authError);

                Debug.Log(failedMessage);
            }
            else
            {
                // Get The User After Registration Success
                user = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = GetAuthErrorMessage("Profile update Failed! Becuase  ", authError);

                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log("Registration Sucessful Welcome " + user.DisplayName);
                    UIManager.Instance.OpenLoginPanel();
                }
            }
        }
    }
    #endregion

    #region Firebase Methods
    /// <summary>
    /// Is being used to get error message from Auth.
    /// </summary>
    /// <param name="DefaultString"></param>
    /// <param name="authError"></param>
    /// <returns></returns>
    private string GetAuthErrorMessage(string DefaultString,AuthError authError)
    {
        switch (authError)
        {
            case AuthError.InvalidEmail:
                DefaultString += "Email is invalid";
                break;
            case AuthError.WrongPassword:
                DefaultString += "Wrong Password";
                break;
            case AuthError.MissingEmail:
                DefaultString += "Email is missing";
                break;
            case AuthError.MissingPassword:
                DefaultString += "Password is missing";
                break;
            default:
                DefaultString += "Login Failed";
                break;
        }
        return DefaultString;
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        // Check if there is a user signed in
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }
    #endregion

}
