using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    #region Variables and Properties
    public static UIManager Instance;

    [SerializeField]
    private GameObject loginPanel;
    
    [SerializeField]
    private GameObject registrationPanel;

    public Text ScoreText;
    public Text HealthText;
    #endregion

    #region Initialization Methods
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "GameScene")
        {
            HealthText.text = HealthHandler.Instance.GetHealth().ToString();
        }
        
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
    }
    #endregion

    #region UI Methods

    public void OpenLoginPanel()
    {
        loginPanel.SetActive(true);
        registrationPanel.SetActive(false);
    }

    public void OpenRegistrationPanel()
    {
        registrationPanel.SetActive(true);
        loginPanel.SetActive(false);
    }

    public void OpenScoreboard()
    {
        SceneManager.LoadScene("Scoreboard");
    }

    public void OpenGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenGameOver()
    {
        SceneManager.LoadScene("DeathScene");
    }
    #endregion


}
