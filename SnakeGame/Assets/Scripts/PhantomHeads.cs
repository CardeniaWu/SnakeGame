using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomHeads : MonoBehaviour
{
    public Transform CollisionPrefab;
    private Transform collisionOccurence;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);



        if (other.tag == "Food")
        {
            GameObject.Find("Snake").GetComponent<Snake>().Grow();
        } else if (other.tag == "Obstacle")
        {
            string destroyedGameObject = this.gameObject.name.ToString();
            Vector3 destroyedGameObjectLocation = this.gameObject.transform.position;
            
            Destroy(this.gameObject);

            collisionOccurence = Instantiate(this.CollisionPrefab);

            Debug.Log($"The collision coordinates are {this.collisionOccurence.transform.position} and {destroyedGameObject} was at {destroyedGameObjectLocation}.");

            Time.timeScale = 0;
        }
    }
}
