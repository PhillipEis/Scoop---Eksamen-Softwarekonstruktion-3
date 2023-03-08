using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    #region Variables and Properties
    private int score = 0;
    public static ScoreHandler Instance;
    #endregion

    #region Initialization Methods
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
    }
    #endregion

    #region Score Methods
    public void AddScore(int newScore)
    {
        score += newScore;
        UIManager.Instance.ScoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
    #endregion
}
