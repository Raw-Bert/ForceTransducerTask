using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public float max;
    public float tempMax;
    public GameObject player;
    public GameObject manager;
    public float percentOfMax;

    public float current;

    public float height;

    public float speed;

    public bool over = false;

    public bool go;
    int MovingAverageLength = 10;
    int value;
    private float movingAverage;

    public int blockTrack = 0;
    public string input;

    int count;

    private float forceBias;

    Vector3 startPosition = new Vector3(0,0,0);

    public GameObject pausedObject;

    GameObject[] forcefromMenu;

    public List<float> averagePoints;
    [SerializeField] private Text coinsLastBlock;

    SerialPort sp = new SerialPort("COM3/aio",9600);

    void Awake(){
        Time.timeScale = 0.0f;
        go = false;

        forcefromMenu = GameObject.FindGameObjectsWithTag("NoDestroy");
        tempMax = forcefromMenu[0].GetComponent<MoveForceData>().force;

        forceBias = forcefromMenu[0].GetComponent<MoveForceData>().bias;

        max = tempMax * percentOfMax; //Sets the actual max to be x% of the max. This is so users do not overexert themselves.

        //get screen size in game units
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

        height = edgeVector.y * 2;

        pausedObject.SetActive(true);

        //Open port
        sp.Open();
        sp.ReadTimeout = 1;
    }

    void Update()
    {
        if(go == true)
        {
            //Move player to the right at constant speed
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            //Move the player to a Y position on screen based on the max force value.
            if(sp.IsOpen)
            {
                if(over == false) 
                {
                    try
                    {
                        input = sp.ReadLine(); 

                        //If inputs equal something, convert input to float and add to running average.
                        if(input != "" || input != null)
                        {
                            averagePoints.Add((float.Parse(input) - forceBias));
                            
                            //If running average has more than 10 items, remove the first one
                            if(averagePoints.Count > 10)
                            {
                                averagePoints.Remove(averagePoints[0]);
                            }

                            current = 0;
                            foreach (float f in averagePoints)
                            {
                                current += f;
                            }
                            current = current / 10;
            //                count ++;
            //                if (count > MovingAverageLength)
            //                {
            //                    movingAverage = movingAverage + (float.Parse(input) - movingAverage) / (MovingAverageLength + 1);
            //                }
            //                else 
            //                    movingAverage += float.Parse(input);
            //               
            //                if (count == MovingAverageLength)
            //                {
            //                    movingAverage = movingAverage / count;
            //                    //count = 0;
            //                    
            //     //Debug.Log("Moving Average: " + movingAverage); //for testing purposes
            //                }
            //                current = movingAverage;
//
            //                    //current = float.Parse(input);
            //                //Debug.Log("Current: " + current);
            //                //Debug.Log("Count: " + count);
            //                
                         }
                            //current = float.Parse(input);

                        
                        //Debug.Log("Current: " + current);
                        
                        else{
                    
                        }
                    //Debug.Log(sp.ReadByte() / 2.20462);
                
                    }
                    catch(System.Exception)
                    {
                        //throw;
                    }
                }
            
            //clamp player within the screen
            if(current > max)
            {
                current = max;
            }

            if (current < height - height)
            {
                current = height - height;
            }

            player.transform.position = new Vector3(player.transform.position.x ,
                ((height - height) + (current - (max/2)) / (max/height)), player.transform.position.z);
            }

            else{
                
            }
        }
        else //go == false
        {
            //If left mouse button pressed, pause reset coins per block(coinNumber) and allowtime and player to resume
            if(Input.GetButton("Fire1") && over == false)
            {
                pausedObject.SetActive(false);
                this.GetComponent<CoinPickup>().coinNumber = 0;
                go = true;
                Time.timeScale = 1.0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EndZone")
        {
            manager.GetComponent<EndCondition>().EndBlock(1f);
            Debug.Log("blockTrack value on end: " + blockTrack);
           
            over = true;
            go = false;
        }

        else if (other.tag == "RestZone")
        {
            go = false;
            Time.timeScale = 0.0f;
            if(SceneManager.GetActiveScene().name != "LabViewScene" && SceneManager.GetActiveScene().name != "LabViewTransferScene")
            {   
                coinsLastBlock.gameObject.SetActive(true);
                coinsLastBlock.text = "Coins Collected Last Block: " +   this.GetComponent<CoinPickup>().coinNumber;
            }
            else
            {
                coinsLastBlock.gameObject.SetActive(false);
            }
            player.transform.position = startPosition;
            pausedObject.SetActive(true);
            
            blockTrack += 1;
            
            //Destroy red lines in list then clear the list (for some reason it didn't work without a for loop)
            if(manager.GetComponent<DrawWhiteLines>().redLineList.Count > 0)
            {
                foreach(LineRenderer redLineRenderer in manager.GetComponent<DrawWhiteLines>().redLineList)
                {
                    Destroy(redLineRenderer.gameObject);
                }

                for(int i = 0; i < manager.GetComponent<DrawWhiteLines>().redLineList.Count; i++)
                {
                    manager.GetComponent<DrawWhiteLines>().redLineList.Clear();
                }
            }            
            
            //Delete current white line and make it so next white line can be created
            Destroy(manager.GetComponent<DrawWhiteLines>().currentLine.gameObject);
            manager.GetComponent<DrawWhiteLines>().lineCreated = false;

            //Delete current yellow line and make it so next yellow line can be created
            if(SceneManager.GetActiveScene().name == "GamifiedScene" || SceneManager.GetActiveScene().name == "LabViewScene")
            {
                Destroy(manager.GetComponent<DrawLine>().currentLine.gameObject);
                manager.GetComponent<DrawLine>().birdStart = true;
            }

            //Enable coins to start spawning from 0
            manager.GetComponent<DrawWhiteLines>().lastCoin = 0;

            //Destroy all coins in list
            if(manager.GetComponent<DrawWhiteLines>().coinList.Count > 0)
            {
                foreach(GameObject cn in manager.GetComponent<DrawWhiteLines>().coinList)
                {
                    Destroy(cn);
                }
            }

            //Calls function from DrawWhiteLines Script using the value in blocktrack to determine which block to draw next
            manager.GetComponent<DrawWhiteLines>().DrawNextLine(manager.GetComponent<DrawWhiteLines>().randomList[blockTrack]);
        }
    }
    
}
