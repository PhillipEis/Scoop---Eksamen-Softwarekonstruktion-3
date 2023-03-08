using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using System;

public class FirestoreManager : MonoBehaviour
{
    #region Variables and Properties
    private FirebaseFirestore db;
    public static FirestoreManager Instance;
    #endregion

    #region Initialization Methods
    private void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        StartCoroutine(LoadHighScoreAsync());
    }

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region Load Highscore
    /// <summary>
    /// LoadHighScore and set Ref highscore to highscore, it needs to be running in a coroutine
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadHighScoreAsync()
    {
        // Check is UserID is null.
        if (References.UserID != null)
        {
            // Read get data from the user.
            var loadTask = db.Document("users/" + References.UserID).GetSnapshotAsync();
            yield return new WaitForSeconds(1);
            // Check if completed
            if (loadTask.IsCompleted)
            {
                DocumentSnapshot snapshot = loadTask.Result;
                if (snapshot.Exists)
                {
                    // Try to get the value out, as object.
                    object highscore;
                    if (snapshot.TryGetValue("HighScore", out highscore))
                    {
                        // Set Ref HighScore to highscore from db, we need to Convert it to Int.
                        References.HighScore = Convert.ToInt32(highscore);
                    }
                }
            }
        }
    }
    #endregion

    #region Add Highscore
    /// <summary>
    /// Function to AddHighScore.
    /// </summary>
    /// <param name="highScore"></param>
    public void AddHighScore(int highScore)
    {
        StartCoroutine(AddHighScoreAsync(highScore));
    }
    /// <summary>
    /// Add's the highscore to Firebase, needs to be running in a Coroutine
    /// </summary>
    /// <param name="highScore"></param>
    /// <returns></returns>
    private IEnumerator AddHighScoreAsync(int highScore)
    {
        // Check if UserID or Username is null.
        if (References.UserID != null && References.UserName != null)
        {
            // Create new user, with data from Ref and the new highscore
            User user = new User() { UserID = References.UserID, UserName = References.UserName, HighScore = highScore };
            DocumentReference docRef = db.Collection("users").Document(References.UserID);
            yield return new WaitForSeconds(1);
            // Set data in
            docRef.SetAsync(user).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Failed to update data");
                }
                else
                {
                    References.HighScore = highScore;
                }
            });
        }
    #endregion
    }
}
