using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    #region Variables and Properties
    [SerializeField] Transform[] spawnPoints;
    private float minDelay = 1f;
    private float maxDelay = 2f;
    private int listSize = 10;
    private List<GameObject> spawnList;
    [SerializeField] GameObject[] fruitPrefabs;
    #endregion
    #region Initialization Methods
    // Start is called before the first frame update
    void Start()
    {
        // Create the list of fruits
        spawnList = new List<GameObject>(listSize);
        for (int i = 0; i < listSize; i++)
        {
            // Create a random fruit
            int prefabIndex = Random.Range(0, fruitPrefabs.Length);
            GameObject fruit = Instantiate(fruitPrefabs[prefabIndex]);
            fruit.SetActive(false);
            spawnList.Add(fruit);
        }
        StartCoroutine(SpawnFruits());
    }
    #endregion

    #region Spawning Methods
    /// <summary>
    /// SpawnFruite is a coroutine that spawns a fruit at a random spawn point
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnFruits()
    {
        while (true)
        {   
            // Wait for a random amount of time
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            int fruitIndex = Random.Range(0, spawnList.Count);
            GameObject fruit = spawnList[fruitIndex];
            // Check if the fruit is active
            if (fruit && !fruit.gameObject.activeSelf)
            {
                // Set the fruit to active
                fruit.SetActive(true);
                // Set the position and rotation of the fruit
                fruit.transform.position = spawnPoint.position;
                fruit.transform.rotation = spawnPoint.rotation;

            }
        }
    }
    #endregion
}
