using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        //We randomize the position of the food gameObject through our RandomizePosition function
        RandomizePosition();
    }

    //This function randomizes the food position in the grid area on whole numbers and within the confines of a predefined gridArea that is tied externally to an invisible gameObject that ensures our
    //bounds for randomization does not exceed that of our play area
    private void RandomizePosition()
    {
        //We create a bounds variable to store our gridArea data
        Bounds bounds = this.gridArea.bounds;

        //We set random values for the x & y variables within the bounds max/min ranges
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        //We set the position of the gameObject this script is attached to based on the Vector 3 value of a rounded X, y value
        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }

    //We set up a trigger condition for player collision with food and call the RandomizePosition function to spawn another food asset on a random tile in the play area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RandomizePosition();
            GameObject.Find("GameHandler").GetComponent<GameHandler>().PlayerScored();
        } else if (other.tag == "Phantom")
        {
            RandomizePosition();
            GameObject.Find("GameHandler").GetComponent<GameHandler>().PlayerScored();
        }
    }
}
