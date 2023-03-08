using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    #region Variables and Properties
    public static HealthHandler Instance;

    private int health = 3;
    #endregion

    #region Initialization Methods
    private void Awake()
    {
        if(Instance == null && Instance != this)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Health methods
    /// <summary>
    /// RemoveHealth from the user, if health comes under 0, open gameover screen and check if the score is higher than the highscore, if it is, add it to the db.
    /// </summary>
    public void RemoveHealth()
    {
        // Remove health
        health--;
        // Update UI
        UIManager.Instance.HealthText.text = health.ToString();
        // Check if health is under 0
        if(health <= 0)
        {
            // Open gameover screen
            UIManager.Instance.OpenGameOver();
            // Check if score is higher than highscore
            int score = ScoreHandler.Instance.GetScore();
            if(score > References.HighScore)
            {
                // Add score to db
                FirestoreManager.Instance.AddHighScore(score);
            }
        }
    }
    /// <summary>
    /// Returns health as int
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return health;
    }
    #endregion
}
