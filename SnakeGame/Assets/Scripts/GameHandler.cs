using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    
    [Header("Score Systems")]
    //We create the public variable to store our PlayerScore value
    private int PlayerScore;
    //We create another variable to store our player score text
    public GameObject PlayerScoreText;
    
    [Header("Life Systems")]
    //We create variables to store our life game objects
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject loseMenu;
    public GameObject finalScore;

    [Header("PwrUp Systems")]
    //We declare a transform to store our prefab in
    public Transform PhantomHeadPwrUpPrefab;
    //We create a transform variable to hold our pwrUp upon instantiation
    private Transform pwrUp;
    //We declare a waitTime of 15 before our power up can spawn 
    public float waitTime = 15.0f;
    //We declare a destroyTime of 10 before our power up disappears after spawning
    public float destroyTime = 10.0f;
    //Deployment time will track the time before we can deploy
    public float deploymentTimer = 0.0f;
    //Activation time will track the time the gameObject is visible 
    public float activationTimer = 0.0f;
    //We set a bool to test whether the PhantomHeadPwrUp is active or not
    public bool isBuffActive;
    //Set a bool to store whether the player is buffed
    public bool isPlayerBuffed;
    //We declare a bool to track whether the pwrUp object has a collision with the player
    public bool coll;


    void Start()
    {
        Debug.Log("GameHandler.Start");
    }

    void Update()
    {
        //First we check to see if deploymentTimer has exceed waitTime
        if ((deploymentTimer > waitTime) && (isBuffActive == false))
        {
            StartCoroutine("buffActivation");
        }

        if (isBuffActive == true)
        {
            activeBuff();
        }

        if ((GameObject.Find("Snake").GetComponent<Snake>().phantomHead1 != null) || (GameObject.Find("Snake").GetComponent<Snake>().phantomHead2 != null))
        {
            isPlayerBuffed = true;
        } else
        {
            isPlayerBuffed = false;
        }
    }


    private void FixedUpdate()
    {
        //Check to see whether our PhantomHeadPwrUp buff is active.
        //If not, we set our deploymentTimer equal to Time.deltaTime
        if ((isBuffActive == false) && (isPlayerBuffed == false))
        {
            deploymentTimer += Time.fixedDeltaTime;
        } else if ((isBuffActive == true) && isPlayerBuffed == false)
        {
            activationTimer += Time.fixedDeltaTime;
        }
    }


    //We create a function to handle the logic for when a player scores
    public void PlayerScored()
    {
        PlayerScore++;
        PlayerScoreText.GetComponent<TextMeshProUGUI>().text = PlayerScore.ToString();
    }
    
    //We create a function to reset the PlayerScore to 0
    public void ResetScore()
    {
        PlayerScore = 0;
        PlayerScoreText.GetComponent<TextMeshProUGUI>().text = PlayerScore.ToString();
    }

    //We create a function to Restart the state of our game and set the game time back to 1
    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1f;
    }

    //We create a function to send us back to the Main Menu and set the game time back to 1
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    //We create a function to exit the game
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }
    
    //We create a function to deactivate our life game objects
    public void LoseLife()
    {
        //Decrease the player score if the player score is greater than 0
        if (PlayerScore > 0)
        {
            PlayerScore--;
            PlayerScoreText.GetComponent<TextMeshProUGUI>().text = PlayerScore.ToString();

        }
        if (life3)
        {
            Destroy(life3);
        } 
        else if (life2)
        {
            Destroy(life2);
        }
        else
        {
            Destroy(life1);
            loseMenu.SetActive(true);
            finalScore.GetComponent<TextMeshProUGUI>().text = PlayerScore.ToString();
            Time.timeScale = 0f;
        }
    }
    
    //We use a coroutine to prevent this code from running within a single frame update
    IEnumerator buffActivation()
    {
            while (isBuffActive == false)
            {
                //Here we produce a random range between 0 and 10000
                int rand = Random.Range(0, 10000);
                
                //Here we test whether the rand variable produced is greater than or equal to 9999.
                //I had a hard time creating a decent delay between the end of the deployment timer and buffActivation so could certainly improve my method.
                //There's likely a better way to code it
                if (rand >= 9999)
                {

                    //We instantiate the PhantomHeadPwrUp gameObject
                    pwrUp = Instantiate(this.PhantomHeadPwrUpPrefab);

                    //We reset the deploymentTimer variable to 0
                    deploymentTimer = 0.0f;

                    //We set the variable isBuffActive to true
                    isBuffActive = true;
                }
            //Here we return the control back to unity and set a delay of 2 seconds before resuming the while loop. 
            yield return new WaitForSeconds(2f);
        }
    }

    private void activeBuff()
    {
        //We test whether our activation time has expired or if the player collided with the pwrUp
        if ((activationTimer >= destroyTime) || (coll == true))
        {
            //We destroy the gameObject
            Destroy(pwrUp.gameObject);
            //We set isBuffActive to false to restart the deployment timer
            isBuffActive = false;
            //We set the activation timer back to 0
            activationTimer = 0.0f;
            //We ensure coll returns to false
            coll = false;
        }
    }
}
