using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    //variable for storing user input and translating that into player movement
    private Vector2 _direction = Vector2.right;
    //variable for storing the segment data
    private List<Transform> _segments;
    //variable for storing the prefab segment asset
    public Transform segmentPrefab;
    public Transform initialTwoSegmentPrefab;
    private int segmentCount;

    [Header("Buff Systems")]
    //We declare an activation time
    public float activationTime = 10.0f;
    //We declare a timer to track the time the buff is active
    public float buffTimer;
    //We create a variable to hold our phantomHeadPrefab
    public Transform phantomHeadPrefab1;
    public Transform phantomHeadPrefab2;
    //We create two variables to hold our phantomHeads after instantiation
    public Transform phantomHead1;
    public Transform phantomHead2;

    [Header("Life Systems")]
    //We create a variable for our lives
    public int lives = 3;


    private void Start()
    {
        //We create the _segment list and add the transform of the game object to the list
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }

    private void Update()
    {

        //This if loop intakes player input in the form of WASD & translates it into player movement
        if (Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }

        if (GameObject.Find("GameHandler").GetComponent<GameHandler>().isPlayerBuffed == true)
        {
            if ((_direction == Vector2.up) || (_direction == Vector2.down))
            {
                //We set the transform point of phantomHead1 one tile to the right and phantomHead 2 one tile to the left
                if (phantomHead1 != null)
                {
                    phantomHead1.transform.position = transform.TransformPoint(1, 0, 0);
                }

                if (phantomHead2 != null)
                {
                    phantomHead2.transform.position = transform.TransformPoint(-1, 0, 0);
                }
            }
            else if ((_direction == Vector2.left) || (_direction == Vector2.right))
            {
                //We set the transform point of phantomHead1 one tile to the bottom and phantomHead 2 one tile to the top
                if (phantomHead1 != null)
                {
                    phantomHead1.transform.position = transform.TransformPoint(0, 1, 0);
                }
                
                if (phantomHead2 != null)
                {
                    phantomHead2.transform.position = transform.TransformPoint(0, -1, 0);
                }
            }

            //We check to see if the buffTimer exceeds the activationTime
            if (buffTimer > activationTime)
            {
                //If so, we destroy both gameObjects that have been instantiated
                if (phantomHead1 != null)
                {
                    Destroy(phantomHead1.gameObject);
                }
                
                if (phantomHead2 != null)
                {
                    Destroy(phantomHead2.gameObject);
                }

                //And reset buffTimer back to 0
                buffTimer = 0.0f;
            }
        }
        
    }

    private void FixedUpdate()
    {
        //This for loop constantly updates the segments to ensure they move consistently with our player character
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
            );

        //We check the var isBuffActive from the GameHandler script and activate the bufftimer if it is
        if (GameObject.Find("GameHandler").GetComponent<GameHandler>().isPlayerBuffed == true)
        {
            //We subscribe our buffTimer to fixedDeltaTime
            buffTimer += Time.fixedDeltaTime;
        }
    }


    //We create a grow function to add a segment to our snake
    public void Grow()
    {
        
        if (_segments.Count < 10)
        {
            //We create a new variable "segment" to hold our newly instantiated prefab asset
            Transform initialSegments = Instantiate(this.initialTwoSegmentPrefab);
            //Put the position as the _segments position that is currently last
            initialSegments.position = _segments[_segments.Count - 1].position;

            //And add this variable to the _segments list
            _segments.Add(initialSegments);
        } else
        {
            //We create a new variable "segment" to hold our newly instantiated prefab asset
            Transform segment = Instantiate(this.segmentPrefab);
            //Put the position as the _segments position that is currently last
            segment.position = _segments[_segments.Count - 1].position;

            //And add this variable to the _segments list
            _segments.Add(segment);
        }
    }

    //We create a collision check to see whether the player runs into an object and then detemine what that object is and what it does
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If we hit food, grow, if we hit an obstacle, reset game
        if (other.tag == "Food")
        {
            Grow();
        } else if (other.tag == "Obstacle")
        {
            //We need to decrease our life count
            GameObject.Find("GameHandler").GetComponent<GameHandler>().LoseLife();

            //We need to destroy our last segment and decrease our segment count
            segmentCount = _segments.Count -1;
            if (segmentCount > 0)
            {
                for (int i = 1; i < _segments.Count; i++)
                {
                    Destroy(_segments[i].gameObject);
                }

                //Here we clear the _segments list of values
                _segments.Clear();
                _segments.Add(this.transform);

                //We set the transform of this back to 0
                this.transform.position = Vector3.zero;

                //here we will reinstantiate all the segments minus the one we lost
                for (int i = 1; i < segmentCount; i++)
                {
                    if (_segments.Count < 10)
                    {
                        //We create a new variable "segment" to hold our newly instantiated prefab asset
                        Transform initialSegments = Instantiate(this.initialTwoSegmentPrefab);
                        //Put the position as the _segments position that is currently last
                        initialSegments.position = _segments[_segments.Count - 1].position;

                        //And add this variable to the _segments list
                        _segments.Add(initialSegments);
                    }
                    else
                    {
                        //We create a new variable "segment" to hold our newly instantiated prefab asset
                        Transform segment = Instantiate(this.segmentPrefab);
                        //Put the position as the _segments position that is currently last
                        segment.position = _segments[_segments.Count - 1].position;

                        //And add this variable to the _segments list
                        _segments.Add(segment);
                    }
                }
            }
            else
            {
                this.transform.position = Vector3.zero;
            }
        } else if (other.tag == "PwrUp")
        {
            PhantomHeads();
        }
    }

    private void PhantomHeads()
    {
        //Instantiate SnakeHeads Prefab
        phantomHead1 = Instantiate(this.phantomHeadPrefab1);
        phantomHead2 = Instantiate(this.phantomHeadPrefab2);

        //Set prefab position to be adjacent to snake head
            if ((_direction == Vector2.up) || (_direction == Vector2.down))
            {
                //We set the transform point of phantomHead1 one tile to the right and phantomHead 2 one tile to the left
                phantomHead1.transform.position = transform.TransformPoint(1, 0, 0);
                phantomHead2.transform.position = transform.TransformPoint(-1, 0, 0);
            }
            else if ((_direction == Vector2.left) || (_direction == Vector2.right))
            {
                //We set the transform point of phantomHead1 one tile to the bottom and phantomHead 2 one tile to the top
                phantomHead1.transform.position = transform.TransformPoint(0, 1, 0);
                phantomHead2.transform.position = transform.TransformPoint(0, -1, 0);
            }


        //Handle phantomHead interaction with other gameObjects (destruction on contact with obstacles or Grow on contact with food)

    }
}
