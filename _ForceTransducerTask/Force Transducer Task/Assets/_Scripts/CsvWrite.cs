using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvWrite : MonoBehaviour {
    
    public GameObject player;
    public GameObject manager;
    public GameObject raycaster;
    private List<string[]> rowData = new List<string[]>();
   

    // Use this for initialization
    void Awake () {

        // Creating First row of titles manually.
        string[] rowDataTemp = new string[5];
        rowDataTemp[0] = "Time (s)";
        rowDataTemp[1] = "White Line Pos";
        rowDataTemp[2] = "User Input";
        rowDataTemp[3] = "Actual Position";
        rowDataTemp[4] = "Margin of Error";
        rowData.Add(rowDataTemp);
    }

     private string getPath(){
         
        return Application.dataPath +"/CSV/Output/"+"Saved_data.csv"; //filepath
    }

    void FixedUpdate() {
        {
            if(player.GetComponent<MovePlayer>().over == false)
            {
                string[] rowDataTemp = new string[5];
  
                // Adds values to cells
                if(player.GetComponent<MovePlayer>().over == false && Time.timeScale == 1){
                    rowDataTemp = new string[5];
                    rowDataTemp[0] = (raycaster.GetComponent<RaycastToLine>().timer).ToString(); //time
                    rowDataTemp[1] = "WLP"; //
                    rowDataTemp[2] = "INPUT"; //raw input // player.GetComponent<MovePlayer>().input;
                    rowDataTemp[3] = (((raycaster.transform.position.y) + (manager.GetComponent<CSVRead>().height/2)) / 100).ToString(); //player position at center
                    rowDataTemp[4] = "Margin of Error"; //% Error (((Actual value - Expected value) / Expected value) * 100) OR See next line
                    //(((raycaster.transform.position.y) + (manager.GetComponent<CSVRead>().height/2)) / 100) - white line value / white line value) *100;
                    rowData.Add(rowDataTemp);
                }

                //Output the data
                string[][] output = new string[rowData.Count][];

                for(int i = 0; i < output.Length; i++)
                {
                    output[i] = rowData[i];
                }

                int length = output.GetLength(0);
                string delimiter = ",";

                StringBuilder sb = new StringBuilder();
        
                for (int index = 0; index < length; index++)
                    sb.AppendLine(string.Join(delimiter, output[index]));
        
                //Writes to and closes file
                string filePath = getPath();
                StreamWriter outStream = System.IO.File.CreateText(filePath);
                outStream.WriteLine(sb);
                outStream.Close();
            }
            //else
                //outStream.Close();
        }
    }
}


