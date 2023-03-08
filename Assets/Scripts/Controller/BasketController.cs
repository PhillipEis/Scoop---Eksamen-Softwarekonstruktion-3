using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class BasketController : MonoBehaviour
{
    #region Variables and Properties
    Camera cam;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        // Get the screen bounds in world space
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        // Get the mouse position in world space
        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        // Set the basket's position to the mouse position
        transform.position = new Vector3(Mathf.Clamp(worldPos.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), transform.position.y);
    }
    // When the basket collides with a fruit, call the fruit's Grab method
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IGrabable grabable = collision.transform.GetComponent<IGrabable>();
        // If the fruit is grabable, call the Grab method with null-conditional operator
        grabable?.Grab();
    }
    #endregion
}
