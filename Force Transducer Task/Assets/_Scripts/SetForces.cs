using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class SetForces : MonoBehaviour
{
    public float maxForce;
    float current;
    public Text maxText;
    public bool settingForce;
    public Text forceBtn;
    public GameObject forceTracker;

    SerialPort sp = new SerialPort("COM3",9600);

    void Awake(){
        forceTracker = GameObject.Find("ForceTracker");
        maxForce = forceTracker.GetComponent<MoveForceData>().force;
        maxText.text = ("Max Force: " + maxForce);
        //maxText.text = ("Max Force: 0"); 
        forceBtn.text = ("Set Max Force");
        //Open port
        sp.Open();
        sp.ReadTimeout = 1;
    }

    void Update()
    {
        if(settingForce == true)
        {
            forceBtn.text = ("Setting Max Force");

            //Sets max force based on inputs
            if(current > maxForce)
            {
                maxForce = current;
                maxText.text = ("Max Force: " + maxForce);
                forceTracker.GetComponent<MoveForceData>().force = maxForce;
            }
            if (current < 0)
            {
                current = 0;
            }


            if(sp.IsOpen)
            {
                try
                {
                    string input = sp.ReadLine(); 
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
            //if(!sp.IsOpen)
                //sp.Open();
        }
        else
        {
            forceBtn.text = ("Set Max Force");
        }
    }
    void OnDestroy()
    {
        sp.Close();
    }
}
