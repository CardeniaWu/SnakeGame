using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomHeadPwrUp : MonoBehaviour
{
    //We declare our gridArea variable publicly to store our spawn area bounds
    public BoxCollider2D gridArea;

    // Start is called before the first frame update
    void Start()
    {
        gridArea = GameObject.Find("GridArea").GetComponent<BoxCollider2D>();
        RandomizePosition();
    }

    //This function randomizes the PhantomHeadPwrUp position in the grid area on whole numbers and within the confines of a predefined gridArea that is tied externally to an invisible gameObject that ensures our
    //bounds for randomization does not exceed that of our play area
    public void RandomizePosition()
    {
        //We create a bounds variable to store our gridArea data
        Bounds bounds = this.gridArea.bounds;

        //We set random values for the x & y variables within the bounds max/min ranges
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        //We set the position of the gameObject this script is attached to based on the Vector 3 value of a rounded X, y value
        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);
    }



    // Update is called once per frame
    void Update()
    {
        if (this.transform.position == GameObject.Find("Food").transform.position)
        {
            RandomizePosition();
        }
    }

    //We test to see whether the player collides with the pwrUp
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //We set the bool coll to true
            GameObject.Find("GameHandler").GetComponent<GameHandler>().coll = true;
        }
    }
}
