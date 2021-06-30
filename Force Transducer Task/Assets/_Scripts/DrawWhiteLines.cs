using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class DrawWhiteLines : MonoBehaviour
{
    Vector2 tempLineStart; // Start position of red line
    Vector2 tempLineEnd;
    Vector2 tempLineEndBottom;

    public GameObject line;
    public GameObject currentLine;

    public LineRenderer lineRenderer;
    public List<Vector2> lp;
    
    public EdgeCollider2D edgeCollider;



    public GameObject redLine;
    public GameObject currentLineRed;

    public LineRenderer redLineRenderer;
    public List<Vector2> lpRed;

    public List <LineRenderer> redLineList;
    
    //These lists are made to be cleared out after each block
    public List<float> lineX; //X values of white line
    public List<float> lineY; // Y values of white line
    public List<GameObject> redLines = new List<GameObject>(); //List of red lines
    public List<GameObject> coinList = new List<GameObject>(); //List of coins

    public GameObject player;
    public GameObject coin;
    public GameObject endZone;
    public GameObject restZone;
    //private int numberOfBlocks;
    public bool lineCreated = false;

    GameObject[] forcefromMenu;
    public float tempMax;
    public float height;
    //public List<int> randomTracker = new List<int>();
    public float lastCoin = 0;

    public string gamifiedScene;
    public string gamifiedTransferScene;

    public List<int> randomList;

    public int numberOfBlocks;
    public string localDate;

    void Awake()
    {
        Debug.Log("Do the thing");
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

        forcefromMenu = GameObject.FindGameObjectsWithTag("NoDestroy");
        numberOfBlocks = forcefromMenu[0].GetComponent<MoveForceData>().blocks;
        tempMax = forcefromMenu[0].GetComponent<MoveForceData>().force;
        //numberOfBlocks = forcefromMenu[0].GetComponent<MoveForceData>().blocks;

        height = edgeVector.y * 2;

        this.GetComponent<CSVRead>().ReadCSVFiles(numberOfBlocks);
        randomList = this.GetComponent<CSVRead>().randomTracker;
        
        Debug.Log("Random Tracker at 0: " + randomList[0]);
        DrawNextLine(randomList[0]);
    }

    public void DrawNextLine(int blockNum)
    {
        localDate = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        Debug.Log("Now drawing block: " + blockNum);
        lineX.Clear();
        lineY.Clear();
        
        /// - Take the appropriate CSV data and input it into the list which gets drawn - ///
        if(blockNum == 1)
        {
            for(int k = 0; k < this.GetComponent<CSVRead>().csvX.Count; k++)
            {
                lineX.Add(this.GetComponent<CSVRead>().csvX[k]);
                lineY.Add((this.GetComponent<CSVRead>().csvY[k]) * (100) - (height/2));
            }
        }

        else if(blockNum == 2)
        {
            for(int k = 0; k < this.GetComponent<CSVRead>().csvX2.Count; k++)
            {
                lineX.Add((this.GetComponent<CSVRead>().csvX2[k]));
                lineY.Add((this.GetComponent<CSVRead>().csvY2[k]) * (100) - (height/2));
            }
        }

        else if(blockNum == 3)
        {
            for(int k = 0; k < this.GetComponent<CSVRead>().csvX3.Count; k++)
            {
                lineX.Add((this.GetComponent<CSVRead>().csvX3[k]));
                lineY.Add((this.GetComponent<CSVRead>().csvY3[k]) * (100) - (height/2));
            }
        }

        else if(blockNum == 4)
        {
            for(int k = 0; k < this.GetComponent<CSVRead>().csvX4.Count; k++)
            {
                lineX.Add((this.GetComponent<CSVRead>().csvX4[k]));
                lineY.Add((this.GetComponent<CSVRead>().csvY4[k]) * (100) - (height/2));
            }
            
        }

        for(int a = 0; a < lineX.Count; a++)
        {
            if(lineX[a] <= 0.0005f && lineCreated == false)
            {
                CreateLine();
                lineCreated = true;
                tempLineStart = new Vector2(lineX[a], lineY[a] + 0.5f);
                tempLineEnd = new Vector2(lineX[a] + 0.01f, lineY[a] + 0.5f);
            }
            else
            {
                if(Vector2.Distance(new Vector2(lineX[a], lineY[a]), lp[lp.Count - 1]) > 0.01f)
                {
                    UpdateLine(new Vector2(lineX[a], lineY[a]));

                    //Debug.Log("ABS Value ystart and current Y: " + Mathf.Abs(tempLineStart.y - lineY[a]));
                    if((Mathf.Abs(tempLineStart.y - (lineY[a] + 0.5f)) > 0.1f) && (SceneManager.GetActiveScene().name != gamifiedScene) && (SceneManager.GetActiveScene().name != gamifiedTransferScene))
                    {
                        if(Mathf.Abs((lineY[a - 1] + 0.5f) - tempLineStart.y) <= 0.1f)// && tempLineEnd.x != tempLineStart.x)
                        {
                            tempLineEnd = new Vector2(lineX[a - 1], lineY[a - 1] + 0.5f);
                            tempLineEndBottom = new Vector2(lineX[a - 1], lineY[a - 1] - 0.5f);
                            

                            DrawRedLine(tempLineStart, tempLineEnd);
                            DrawRedLine(new Vector2(tempLineStart.x, tempLineStart.y - 1), tempLineEndBottom);
                            //DrawRedLine(tempLineStart, new Vector2(tempLineEnd.x, tempLineEnd.y - 1));

                            tempLineStart = tempLineEnd;
                        
                        }
                        
                        else
                        {
                            tempLineEnd = new Vector2(lineX[a - 1], lineY[a - 1] + 0.5f);
                            tempLineStart = tempLineEnd;
                        }

                    }

                    //If the last coin was a cfertain distance away and it is a gamified scene, place another coin and set the lastcoin position to this coin
                    if (((lineX[a] - lastCoin) > 0.5f) && (SceneManager.GetActiveScene().name == gamifiedScene || SceneManager.GetActiveScene().name == gamifiedTransferScene))
                    {
                        SpawnCoin(new Vector3(lineX[a], lineY[a], 2));
                        lastCoin = lineX[a];
                    }
                }

                if(a == (lineX.Count - 1) && player.GetComponent<MovePlayer>().blockTrack != randomList.Count - 1)
                {
                    endZone.SetActive(false);
                    restZone.SetActive(true);
                    restZone.transform.position = new Vector3((lineX[a] + 2.5f),0,0);

                    if((SceneManager.GetActiveScene().name != gamifiedScene) && (SceneManager.GetActiveScene().name != gamifiedTransferScene))
                    {
                        tempLineEnd = new Vector2(lineX[a - 1], lineY[a - 1] + 0.5f);
                        tempLineEndBottom = new Vector2(lineX[a - 1], lineY[a - 1] - 0.5f);
                        DrawRedLine(tempLineStart, tempLineEnd);
                        DrawRedLine(new Vector2(tempLineStart.x, tempLineStart.y - 1), tempLineEndBottom);
                    }
                }

                else if (a == (lineX.Count - 1) && player.GetComponent<MovePlayer>().blockTrack == randomList.Count - 1)
                {
                    restZone.SetActive(false);
                    endZone.SetActive(true);
                    endZone.transform.position = new Vector3((lineX[a] + 2.5f),0,0);
                    if((SceneManager.GetActiveScene().name != gamifiedScene) && (SceneManager.GetActiveScene().name != gamifiedTransferScene))
                    {
                        tempLineEnd = new Vector2(lineX[a - 1], lineY[a - 1] + 0.5f);
                        tempLineEndBottom = new Vector2(lineX[a - 1], lineY[a - 1] - 0.5f);
                        DrawRedLine(tempLineStart, tempLineEnd);
                        DrawRedLine(new Vector2(tempLineStart.x, tempLineStart.y - 1), tempLineEndBottom);
                    }
                }
                
            }
        }

    }

    //Creates a new white line
    void CreateLine()
    {
        currentLine = Instantiate(line, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        edgeCollider = currentLine.GetComponent<EdgeCollider2D>();

        lp.Clear();
        lp.Add(new Vector2(lineX[0], lineY[0]));
        lp.Add(new Vector2(lineX[0], lineY[0]));

        lineRenderer.SetPosition(0, lp[0]);
        lineRenderer.SetPosition(1,lp[1]);

        edgeCollider.points = lp.ToArray();
        
    }

    //Updates the white line with the new points and connects them
    void UpdateLine(Vector2 newPoint)
    {
        lp.Add(newPoint);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);

        edgeCollider.points = lp.ToArray();

    }

    //Spawn a coin at desired position
    void SpawnCoin(Vector3 pos)
    {
        GameObject cn = Instantiate(coin, pos, Quaternion.identity);
        coinList.Add(cn);
    }

    //Draws red lines
    public void DrawRedLine(Vector2 lineStart, Vector2 lineEnd)
    {
        currentLineRed = Instantiate(redLine, Vector3.zero, Quaternion.identity);
        redLineRenderer = currentLineRed.GetComponent<LineRenderer>();

        redLineRenderer.SetPosition(0, lineStart);
        redLineRenderer.SetPosition(1, lineEnd);

        redLineList.Add(redLineRenderer);
    }

}
