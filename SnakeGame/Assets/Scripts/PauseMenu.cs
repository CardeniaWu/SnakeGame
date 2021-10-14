using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //We create our variables for the pause menu and the bool to evaluate whether game is paused or not
    public GameObject pauseMenu;
    public bool isPaused;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause();
        }
    }


    //We create a function to evaluate whether the game is paused and determine the action thereof

    public void GamePause()
    {
        if (isPaused == false)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        } else
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

}
