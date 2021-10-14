using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleSnakeHeads : MonoBehaviour
{
    //We set a variable to hold our animator
    private Animator pAnimator;

    //We set a buff activation time to determine how long the player can be buffed
    public float playerIsBuffedTime = 10.0f;
    //We set a variable to track the activation time of the player buff
    public float buffActivationTimer = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        pAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (pAnimator != null)
        {
            if (GameObject.Find("GameHandler").GetComponent<GameHandler>().isPlayerBuffed == true)
            {
                pAnimator.SetBool("activation", true);
                GetComponent<Collider2D>().isTrigger = false;
            } else
            {
                pAnimator.SetBool("activation", false);
                GetComponent<Collider2D>().isTrigger = true;
            }
        }

        if (buffActivationTimer >= playerIsBuffedTime)
        {
            //We set the activation timer back to 0
            buffActivationTimer = 0.0f;

            //We set the isPlayerBuffed variable to false
            GameObject.Find("GameHandler").GetComponent<GameHandler>().isPlayerBuffed = false;
        }
    }

    private void FixedUpdate()
    {
        if (GameObject.Find("GameHandler").GetComponent<GameHandler>().isPlayerBuffed == true)
        {
            buffActivationTimer += Time.fixedDeltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //If we hit food, grow, if we hit an obstacle, reset game
        if (other.tag == "Food")
        {
            GameObject.Find("Snake").GetComponent<Snake>().Grow();
        } else if (other.tag == "Obstacle")
        {
            pAnimator.SetBool("activation", false);
        }
    }
}
