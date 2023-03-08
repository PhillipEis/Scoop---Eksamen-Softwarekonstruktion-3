using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    #region Variables and Properties
    [SerializeField] RowUI rowUi;
    #endregion

    #region Initialization Methods
    // Start is called before the first frame update
    async void Start()
    {
        // Load the scores from the database
        Score[] scores = await LoadScoreboard.Instance.QueryScoresAsync();
        for (int i= 0; i < scores.Length; i++)
        {
            // Instantiate a row for each score
            var row = Instantiate(rowUi, transform).GetComponent<RowUI>();
            row.rank.text = (i+1).ToString();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
    #endregion
}
