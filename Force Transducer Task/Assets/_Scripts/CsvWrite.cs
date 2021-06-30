using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class CsvWrite : MonoBehaviour {
    
    public GameObject player;
    public GameObject manager;
    public GameObject raycaster;
    private List<string[]> rowData = new List<string[]>();

    //int blockNumber = 1;
    [SerializeField] private string labViewScene = "LabViewScene";
    [SerializeField] private string labViewTransfer = "LabViewTransferScene";
    [SerializeField] private string gamifiedScene = "GamifiedScene";
    [SerializeField] private string gamifiedTransfer = "GamifiedTransfer";

    // Use this for initialization
    void Awake () {

        // Creating First row of titles manually.
        string[] rowDataTemp = new string[6];
        rowDataTemp[0] = "Time (s)";
        rowDataTemp[1] = "White Line Pos";
        rowDataTemp[2] = "User Input";
        rowDataTemp[3] = "Actual Position";
        rowDataTemp[4] = "Margin of Error";
        rowDataTemp[5] = "Coins Collected";
        rowData.Add(rowDataTemp);
    }

    private string getPath(){
        int tempBlock = player.GetComponent<MovePlayer>().blockTrack + 1;
        if((SceneManager.GetActiveScene().name == labViewScene))
        {
            return Application.dataPath +"/CSV/Output/"+"Labview_block" + tempBlock + "_" + manager.GetComponent<DrawWhiteLines>().localDate + "_data.csv"; //filepath
        }
        else if((SceneManager.GetActiveScene().name == labViewTransfer))
        {
            return Application.dataPath +"/CSV/Output/"+"LabviewTransfer_block" + tempBlock + "_" + manager.GetComponent<DrawWhiteLines>().localDate + "_data.csv"; //filepath
        }
        else if((SceneManager.GetActiveScene().name == gamifiedScene))
        {
             return Application.dataPath +"/CSV/Output/"+"Gamified_block" + tempBlock + "_" + manager.GetComponent<DrawWhiteLines>().localDate + "_data.csv"; //filepath
        }
        else
        {
             return Application.dataPath +"/CSV/Output/"+"GamifiedTransfer_block" + tempBlock + "_" + manager.GetComponent<DrawWhiteLines>().localDate + "_data.csv"; //filepath
        }
    }

    void FixedUpdate() {
        {
            if(player.GetComponent<MovePlayer>().over == false)
            {
                string[] rowDataTemp = new string[6];
  
                // Adds values to cells
                if(player.GetComponent<MovePlayer>().over == false && Time.timeScale == 1){
                    rowDataTemp = new string[6];
                    rowDataTemp[0] = (raycaster.GetComponent<RaycastToLine>().timer).ToString(); //time
                    float wlp = ((raycaster.GetComponent<RaycastToLine>().dist));// White line distance to player
                    rowDataTemp[1] = (((wlp + 1) + (manager.GetComponent<DrawWhiteLines>().height/2)) / 100).ToString(); //Calculate white line distance as position and percentage
                    rowDataTemp[2] = player.GetComponent<MovePlayer>().input; //raw input from arduino
                    rowDataTemp[3] = (((raycaster.transform.position.y + 1) + (manager.GetComponent<DrawWhiteLines>().height/2)) / 100).ToString(); //player position at center
                    rowDataTemp[4] = (Mathf.Abs((((float.Parse(rowDataTemp[3]) - float.Parse(rowDataTemp[1])) / float.Parse(rowDataTemp[1])) * 100) - 100)).ToString();
                    
                    if((SceneManager.GetActiveScene().name == gamifiedScene) || (SceneManager.GetActiveScene().name == gamifiedTransfer))
                    {
                         rowDataTemp[5] = (player.GetComponent<CoinPickup>().coinNumber).ToString();
                    }
                    else
                        rowDataTemp[5] = "N/A";
                   
                    
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


