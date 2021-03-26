using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCondition : MonoBehaviour
{
    public GameObject endScreen;
    public GameObject player;
    public Text coinsCollected;
    public string menuScene;
    Coroutine end;
    //GameObject menuData;
    GameObject[] objs;
    void Awake() 
    {
        endScreen.SetActive(false);
        objs = GameObject.FindGameObjectsWithTag("NoDestroy");
    }

    // Start is called before the first frame update
    public void EndBlock(float seconds)
    {
        player.GetComponent<MovePlayer>().over = true;
        coinsCollected.text = ("Coins Collected: " + player.GetComponent<CoinPickup>().coinNumber);
        end = StartCoroutine(EndGame(seconds));

        
    }

    public void MenuButton()
    {
        //for(int i = 0; i < objs.Length; i++)
        //{
        //    Destroy(objs[i].gameObject);
        //}
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }

    IEnumerator EndGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Time.timeScale = 0f;
        endScreen.SetActive(true);
    }
}
