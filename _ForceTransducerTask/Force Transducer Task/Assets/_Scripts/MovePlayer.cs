using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

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

    public int blockTrack = 0;

    Vector3 startPosition = new Vector3(0,0,0);

    public GameObject pausedObject;

    GameObject[] forcefromMenu;

    SerialPort sp = new SerialPort("COM3",9600);

    void Awake(){
        Time.timeScale = 0.0f;
        go = false;

        forcefromMenu = GameObject.FindGameObjectsWithTag("NoDestroy");
        tempMax = forcefromMenu[0].GetComponent<MoveForceData>().force;

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

        
        //if (GetKeyDown(key))
        //{
            //go == true;
        //}
        //{
            //Move player to the right at constant speed
            transform.Translate(Vector2.right * Time.deltaTime * speed);

            //Move the player to a Y position on screen based on the max force value.
            if(sp.IsOpen)
            {
                if(over == false) 
                {
                    try
                    {
                
                        string input = sp.ReadLine(); 
                        //float current = float.Parse(sp.ReadLine)
                        //Debug.Log(input);
                        if(input != "" || input != null)
                        {
                            current = float.Parse(input);
                        
                        //Debug.Log("Current: " + current);
                        }
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
        else //if go == false
        {
            if(Input.GetButton("Fire1"))
            {

                pausedObject.SetActive(false);
                go = true;
                Time.timeScale = 1.0f;
                
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ENDGAMEEEEEEEEEEE1111");
        if (other.tag == "EndZone")
        {
            manager.GetComponent<EndCondition>().EndBlock(1f);
            Debug.Log("ENDGAMEEEEEEEEEEE");
           
            over = true;
        }
        else if (other.tag == "RestZone")
        {
            go = false;
            blockTrack += 1;
            pausedObject.SetActive(true);
            
            //Delete current white line and make it so next white line can be created
            Destroy(manager.GetComponent<CSVRead>().currentLine.gameObject);
            manager.GetComponent<CSVRead>().lineCreated = false;

            //Delete current yellow line and make it so next yellow line can be created
            Destroy(manager.GetComponent<DrawLine>().currentLine.gameObject);
            manager.GetComponent<DrawLine>().birdStart = true;

            //manager.GetComponent<CSVRead>().

            //Enable coins to start spawning from 0
            manager.GetComponent<CSVRead>().lastCoin = 0;

            if(manager.GetComponent<CSVRead>().coinList.Count > 0)
            {
                foreach(GameObject cn in manager.GetComponent<CSVRead>().coinList)
                {
                    Destroy(cn);
                }
            }
 
            player.transform.position = startPosition;

            manager.GetComponent<CSVRead>().DrawNextLine(manager.GetComponent<CSVRead>().randomTracker[blockTrack]);
            Debug.Log("Random track: " + manager.GetComponent<CSVRead>().randomTracker[blockTrack]);
            Debug.Log("bloackTrack value: " + blockTrack);

            Time.timeScale = 0.0f;
        }
    }
    
}
