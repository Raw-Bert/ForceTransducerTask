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
    public Text TotalCoinsCollected;
    public string menuScene;
    Coroutine end;
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
        coinsCollected.text = ("Coins Collected Last Block: " + player.GetComponent<CoinPickup>().coinNumber);
        TotalCoinsCollected.text = ("Total Coins Collected: " + player.GetComponent<CoinPickup>().totalCoins);
        end = StartCoroutine(EndGame(seconds));
    }

    /*public void MenuButton()
    {
        if(Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }
        SceneManager.LoadScene(menuScene);
    }*/

    IEnumerator EndGame(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Time.timeScale = 0f;
        endScreen.SetActive(true);
    }
}
