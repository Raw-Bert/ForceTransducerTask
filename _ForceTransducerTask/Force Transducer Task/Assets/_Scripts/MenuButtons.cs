using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public InputField overRide;
    public InputField numBlocks;
     public InputField biasField;

    public Text maxText;

    public GameObject menuManager;
    public GameObject forceTracker;
    
    public string trialScene;
    public string labView;
    public string transferView;
    public string gameTransferScene;

    public float overriddenMaxForce;

    void Awake(){
        forceTracker = GameObject.Find("ForceTracker");
    }
    

    public void PlayLabView()
    { 
        //Loads the trial selected from the dropdown. 
        SceneManager.LoadScene(labView);
    }
    public void PlayLabViewTransfer()
    { 
        //Loads the trial selected from the dropdown. 
        SceneManager.LoadScene(transferView);
    }
    public void GamifiedTransfer()
    { 
        //Loads the trial selected from the dropdown. 
        SceneManager.LoadScene(gameTransferScene);
    }
    public void PlayButton()
    { 
        //Loads the trial selected from the dropdown. 
        SceneManager.LoadScene(trialScene);
    }

    public void QuitButton()
    {
        //Only works in the build, not in the Unity editor
        Application.Quit();
    }

    public void ForceButton()
    {
        if (menuManager.GetComponent<SetForces>().settingForce == true)
        {
            menuManager.GetComponent<SetForces>().settingForce = false;
        }
        else 
        {
            //Sets maxForce to 0 before reading in new forces
            menuManager.GetComponent<SetForces>().maxForce = 0;
            menuManager.GetComponent<SetForces>().settingForce = true;
        }
    }

    public void OverrideButton()
    {
        forceTracker.GetComponent<MoveForceData>().force = float.Parse(overRide.text);
        maxText.text = ("Max Force: " +forceTracker.GetComponent<MoveForceData>().force);
        Debug.Log("Force Override: " + forceTracker.GetComponent<MoveForceData>().force);
    }

    public void NumberOfBlocks()
    {
        forceTracker.GetComponent<MoveForceData>().blocks = int.Parse(numBlocks.text);
        Debug.Log("Blocks: " + forceTracker.GetComponent<MoveForceData>().blocks);
    }

    public void SetBias()
    {
        forceTracker.GetComponent<MoveForceData>().bias = float.Parse(biasField.text);
        Debug.Log("Bias: " + forceTracker.GetComponent<MoveForceData>().bias);
    }
}
