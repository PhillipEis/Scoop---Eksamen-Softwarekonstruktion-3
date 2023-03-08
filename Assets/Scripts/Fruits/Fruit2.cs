using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit2 : MonoBehaviour, IGrabable
{
    Renderer render;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    public void Grab()
    {
        ScoreHandler.Instance.AddScore(10);
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!render.isVisible)
        {
            gameObject.SetActive(false);
            HealthHandler.Instance.RemoveHealth();
        }
    }

}
