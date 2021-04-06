using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVRead : MonoBehaviour
{
    public GameObject line;
    public GameObject currentLine;
    public LineRenderer lineRenderer;
    public GameObject lineRed;
    public GameObject currentLineRed;
    public LineRenderer lineRendererRed;
    public List<Vector2> lpRed;
    

    public List<float> lineX;
    public List<float> lineY;

    public List<float> csvX;
    public List<float> csvY;

    public List<float> csvX2;
    public List<float> csvY2;

    public List<float> csvX3;
    public List<float> csvY3;

    public List<float> csvX4;
    public List<float> csvY4;

    public List<Vector2> lp;
    public GameObject player; 

    public EdgeCollider2D edgeCollider;

    public GameObject coin;

    public float lastCoin = 0;
    
    public float height;

    GameObject[] forcefromMenu;
    public float tempMax;

    public List<int> randomTracker = new List<int>();
    //int Lenght = 5;

    public GameObject endZone;
    public GameObject restZone;

    public List<GameObject> coinList = new List<GameObject>();

    //float XOffset = 0;

    int numberOfBlocks;
    

    public bool lineCreated = false;

    // Start is called before the first frame update
    void Awake()
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

        forcefromMenu = GameObject.FindGameObjectsWithTag("NoDestroy");
        tempMax = forcefromMenu[0].GetComponent<MoveForceData>().force;
        numberOfBlocks = forcefromMenu[0].GetComponent<MoveForceData>().blocks;

        height = edgeVector.y * 2;
        //xOffset = 0;
        ReadCSVFiles();
    }

    //get the file paths
    private string getPath(){
         
        return Application.dataPath +"/CSV/Input/"+"Block_1.csv"; //filepath
    }
    private string getPath2(){
         
        return Application.dataPath +"/CSV/Input/"+"Block_2.csv"; //filepath
    }

    private string getPath3(){
         
        return Application.dataPath +"/CSV/Input/"+"Block_3.csv"; //filepath
    }

    private string getPath4(){
         
        return Application.dataPath +"/CSV/Input/"+"Block_4.csv"; //filepath
    }

    //read in the CSV files based on file paths
    void ReadCSVFiles(){
        StreamReader strm = new StreamReader(getPath());
        bool endOfFile = false;
        int i = 0;
        while(!endOfFile)
        {            
            string data_String = strm.ReadLine();

            if(data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(',');

            csvX.Add(float.Parse(data_values[0]));
            csvY.Add(float.Parse(data_values[1]));

            Debug.Log("File 1: " + csvX[i] + " " + csvY[i]);
            i++;
        }

        StreamReader strm2 = new StreamReader(getPath2());
        bool endOfFile2 = false;
        int j = 0;
        while(!endOfFile2)
        {            
            string data_String2 = strm2.ReadLine();

            if(data_String2 == null)
            {
                endOfFile2 = true;
                break;
            }
            var data_values2 = data_String2.Split(',');

            csvX2.Add(float.Parse(data_values2[0]));
            csvY2.Add(float.Parse(data_values2[1]));

            Debug.Log("File 2: " + csvX2[j] + " " + csvY2[j]);

            j++;
        }

        j = 0;

        StreamReader strm3 = new StreamReader(getPath3());
        bool endOfFile3 = false;
        while(!endOfFile3)
        {            
            string data_String3 = strm3.ReadLine();

            if(data_String3 == null)
            {
                endOfFile3 = true;
                break;
            }
            var data_values3 = data_String3.Split(',');

            csvX3.Add(float.Parse(data_values3[0]));
            csvY3.Add(float.Parse(data_values3[1]));

            Debug.Log("File 3: " + csvX3[j] + " " + csvY3[j]);

            j++;
        }

        j = 0;

        StreamReader strm4 = new StreamReader(getPath4());
        bool endOfFile4 = false;
        while(!endOfFile4)
        {            
            string data_String4 = strm4.ReadLine();

            if(data_String4 == null)
            {
                endOfFile4 = true;
                break;
            }
            var data_values4 = data_String4.Split(',');

            csvX4.Add(float.Parse(data_values4[0]));
            csvY4.Add(float.Parse(data_values4[1]));

            Debug.Log("File 4: " + csvX4[j] + " " + csvY4[j]);

            j++;
        }

        randomTracker = new List<int>(new int[numberOfBlocks]);
        int rand;
        for (int a = 0; a < (numberOfBlocks / 4); a++)
        {
            for (int b = 0; b < 4; b++)
            {
                rand = UnityEngine.Random.Range(1,5);
    
                while (randomTracker.Contains(rand))
                {
                    rand = UnityEngine.Random.Range(1,5);
                }
    
                randomTracker[b] = rand;
                print(randomTracker[b]);
            }
        }
        DrawNextLine(randomTracker[0]);

    }
    
    public void DrawNextLine(int blockNum)
    {
        lineX.Clear();
        lineY.Clear();
        if(blockNum == 1)
        {
            for(int k = 0; k < csvX.Count; k++)
            {
                lineX.Add(csvX[k]);
                lineY.Add((csvY[k]) * (100) - (height/2));
            }
        }

        else if(blockNum == 2)
        {
            for(int k = 0; k < csvX2.Count; k++)
            {
                lineX.Add((csvX2[k]));
                lineY.Add((csvY2[k]) * (100) - (height/2));
            }
        }

        else if(blockNum == 3)
        {
            for(int k = 0; k < csvX3.Count; k++)
            {
                lineX.Add((csvX3[k]));
                lineY.Add((csvY3[k]) * (100) - (height/2));
            }
        }

        else if(blockNum == 4)
        {
            for(int k = 0; k < csvX4.Count; k++)
            {
                lineX.Add((csvX4[k]));
                lineY.Add((csvY4[k]) * (100) - (height/2));
            }
            
        }

        for(int a = 0; a < lineX.Count; a++)
        {
            if(lineX[a] <= 0.0005f && !lineCreated)
            {
                CreateLine();
                CreateLineRed();
                lineCreated = true;
            }
            
            else
            {
                if(Vector2.Distance(new Vector2(lineX[a], lineY[a]), lp[lp.Count - 1]) > .1f)
                {
                    UpdateLine(new Vector2(lineX[a], lineY[a]));
                    UpdateLineRed(new Vector2(lineX[a], (lineY[a] - 0.5f)));
                    //UpdateLineRed(new Vector2(lineX[a], (lineY[a] + 0.5f)));                 
//
                    if ((lineX[a] - lastCoin) > 0.5f)
                    {
                        SpawnCoin(new Vector3(lineX[a], lineY[a], 2));
                        lastCoin = lineX[a];
                    }
                }
               

                if(a == (lineX.Count - 1) && randomTracker[player.GetComponent<MovePlayer>().blockTrack] != randomTracker.Count )
                {
                    restZone.transform.position = new Vector3((lineX[a] + 2.5f),0,0);
                }

                else if (randomTracker[player.GetComponent<MovePlayer>().blockTrack] == randomTracker.Count)
                {
                    restZone.SetActive(false);
                    endZone.SetActive(true);
                    endZone.transform.position = new Vector3((lineX[a] + 2.5f),0,0);
                }
                //else{
                    //Do nothing
                //}
            }
        }
        
    }

    //void DrawLines()
    //{
    //    Debug.Log("HERE!!");
    //    float xOffset = 0;
    //    
    //    for (int i = 0; i < (numberOfBlocks / 4); i++)
    //    {
    //        
    //        randomTracker = new List<int>(new int[Lenght]);
    //        int rand;
    //        for (int c = 1; c < Lenght; c++)
    //        {
    //            rand = UnityEngine.Random.Range(1,5);
 //
    //            while (randomTracker.Contains(rand))
    //            {
    //                rand = UnityEngine.Random.Range(1,5);
    //            }
 //
    //            randomTracker[c] = rand;
    //            print(randomTracker[c]);
    //        }
    //        //bool block1Chosen = false;
    //        for (int j = 0; j < randomTracker.Count; j++)
    //        {
    //            float tempOffset = 0;
    //            
    //            int randomNumber = randomTracker[j];
//
    //            Debug.Log(i + "  " + j + "  " + randomNumber);
//
    //            if(randomNumber == 1)
    //            {
    //                for(int k = 0; k < csvX.Count; k++)
    //                {
    //                    lineX.Add(csvX[k] + xOffset);
    //                    lineY.Add((csvY[k]) * (100) - (height/2));
    //                    tempOffset = csvX[k];
    //                }
    //                //xOffset += tempOffset;
    //                Debug.Log("xOffset:  " + xOffset);
    //                Debug.Log("TempOffset:  " + tempOffset);
    //            }
    //            else if(randomNumber == 2)
    //            {
    //                for(int k = 0; k < csvX2.Count; k++)
    //                {
    //                    lineX.Add((csvX2[k]) + xOffset);
    //                    lineY.Add((csvY2[k]) * (100) - (height/2));
    //                    tempOffset = csvX2[k];
    //                }
    //                //xOffset += tempOffset;
    //                Debug.Log("TempOffset:  " + tempOffset);
    //                Debug.Log("xOffset:  " + xOffset);
    //            }
    //            else if(randomNumber == 3)
    //            {
    //                for(int k = 0; k < csvX3.Count; k++)
    //                {
    //                    lineX.Add((csvX3[k]) + xOffset);
    //                    lineY.Add((csvY3[k]) * (100) - (height/2));
    //                    tempOffset = csvX3[k];
    //                }
    //                //xOffset += tempOffset;
    //                Debug.Log("TempOffset:  " + tempOffset);
    //                Debug.Log("xOffset:  " + xOffset);
    //            }
    //            else if(randomNumber == 4)
    //            {
    //                for(int k = 0; k < csvX4.Count; k++)
    //                {
    //                    lineX.Add((csvX4[k]) + xOffset);
    //                    lineY.Add((csvY4[k]) * (100) - (height/2));
    //                    tempOffset = csvX4[k];
    //                }
    //                //xOffset += tempOffset;
    //                //Debug.Log("TempOffset:  " + tempOffset);
    //                
    //            }
    //            xOffset += tempOffset;
    //            Debug.Log("xOffset:  " + xOffset);
    //            //xOffset = lineX[lineX.Count];
    //            //Debug.Log("Count: " + lineX.Count);
    //        }            
    //    }
//
    //    for(int a = 0; a < lineX.Count; a++)
    //    {
    //        if(lineX[a] <= 0.0005f && !lineCreated)
    //        {
    //            CreateLine();
    //            lineCreated = true;
    //        }
    //        else
    //        {
    //            if(Vector2.Distance(new Vector2(lineX[a], lineY[a]), lp[lp.Count - 1]) > .1f)
    //            {
    //                UpdateLine(new Vector2(lineX[a], lineY[a]));
//
    //                if ((lineX[a] - lastCoin) > 0.5f)
    //                {
    //                    SpawnCoin(new Vector3(lineX[a], lineY[a], 2));
    //                    lastCoin = lineX[a];
    //                }
    //            }
    //            if(a == (lineX.Count - 1))
    //            {
    //                Debug.Log("what");
    //                endZone.transform.position = new Vector3((lineX[a] + 2.5f),0,0);
    //            }
    //            //else{
    //                //Do nothing
    //            //}
    //        }
    //    }
    //}

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

    void UpdateLine(Vector2 newPoint)
    {
        lp.Add(newPoint);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);

        edgeCollider.points = lp.ToArray();

    }

    void CreateLineRed()
    {
        currentLineRed = Instantiate(lineRed, Vector3.zero, Quaternion.identity);
        lineRendererRed = currentLineRed.GetComponent<LineRenderer>();

        lpRed.Clear();
        lpRed.Add(new Vector2(lineX[0], lineY[0]));
        lpRed.Add(new Vector2(lineX[0], lineY[0]));

        lineRendererRed.SetPosition(0, lpRed[0]);
        lineRendererRed.SetPosition(1,lpRed[1]);

    }

    void UpdateLineRed(Vector2 newPoint)
    {
        lpRed.Add(newPoint);
        lineRendererRed.positionCount++;
        lineRendererRed.SetPosition(lineRendererRed.positionCount - 1, newPoint);
    }
    

    void SpawnCoin(Vector3 pos)
    {
        GameObject cn = Instantiate(coin, pos, Quaternion.identity);
        coinList.Add(cn);
    }
}
